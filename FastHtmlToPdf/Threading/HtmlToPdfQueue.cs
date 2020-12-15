using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace FastHtmlToPdf.Threading
{
    internal class HtmlToPdfQueue : SynchronizationContext, IComponent, ISynchronizeInvoke
    {
        private Queue<QueueAsyncResult> queue = new Queue<QueueAsyncResult>();
        private volatile static uint threadID = 0;
        private volatile bool disposed = false;
        private readonly object lockObject = new object();
        private SynchronizationContext context;
        private Thread thread;
        private ISite site = null;
        public event EventHandler Disposed;

        public HtmlToPdfQueue()
        {
            InitQueue();

            if (Current == null)
                context = new SynchronizationContext();
            else
                context = Current;
        }

        ~HtmlToPdfQueue()
        {
            Dispose(false);
        }

        private void InitQueue()
        {
            thread = new Thread(Procedure);
            thread.SetApartmentState(ApartmentState.STA);

            lock (lockObject)
            {
                threadID++;
                thread.Start();
                Monitor.Wait(lockObject);
            }
        }

        private void Procedure()
        {
            lock (lockObject)
            {
                Monitor.Pulse(lockObject);
            }

            SetSynchronizationContext(this);
            QueueAsyncResult result = null;

            while (true)
            {
                lock (lockObject)
                {
                    if (disposed)
                        break;

                    if (queue.Count > 0)
                        result = queue.Dequeue();
                    else
                    {
                        Monitor.Wait(lockObject);

                        if (disposed)
                            break;

                        result = queue.Dequeue();
                    }
                }

                result.Invoke();
            }
        }

        public object Invoke(Delegate method, params object[] args)
        {
            if (disposed)
                throw new ObjectDisposedException("Queue");
            else if (method == null)
                throw new ArgumentNullException();

            object returnValue = null;

            if (InvokeRequired)
            {
                var result = new QueueAsyncResult(this, method, args);

                lock (lockObject)
                {
                    queue.Enqueue(result);
                    Monitor.Pulse(lockObject);
                }

                returnValue = EndInvoke(result);
            }
            else
                returnValue = method.DynamicInvoke(args);

            return returnValue;
        }

        #region IComponent
        public ISite Site
        {
            get { return site; }
            set { site = value; }
        }

        public void Dispose()
        {
            if (disposed)
                return;

            Dispose(true);
            OnDisposed(EventArgs.Empty);
        }
        #endregion

        #region ISynchronizeInvoke
        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            throw new NotImplementedException();
        }

        public object EndInvoke(IAsyncResult result)
        {
            if (disposed)
                throw new ObjectDisposedException("Queue");
            else if (!(result is QueueAsyncResult))
                throw new ArgumentException();
            else if (((QueueAsyncResult)result).Owner != this)
                throw new ArgumentException();

            result.AsyncWaitHandle.WaitOne();

            var r = (QueueAsyncResult)result;

            if (r.Error != null)
                throw r.Error;

            return r.ReturnValue;
        }

        public bool InvokeRequired
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId != thread.ManagedThreadId;
            }
        }
        #endregion

        #region IDispose
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (lockObject)
                {
                    disposed = true;

                    Monitor.Pulse(lockObject);

                    GC.SuppressFinalize(this);
                }
            }
        }

        protected virtual void OnDisposed(EventArgs e)
        {
            var handler = Disposed;

            if (handler != null)
            {
                context.Post(delegate (object state)
                {
                    handler(this, e);
                }, null);
            }
        }
        #endregion
    }
}
