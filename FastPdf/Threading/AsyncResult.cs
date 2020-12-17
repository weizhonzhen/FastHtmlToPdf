using System;
using System.Threading;

namespace Fast.Pdf.Threading
{
    internal class AsyncResult : IAsyncResult
    {
        private object owner;
        private AsyncCallback callback;
        private object state;
        private ManualResetEvent waitHandle = new ManualResetEvent(false);
        private bool completedSynchronously;
        private bool isCompleted = false;
        private int threadId;

        public AsyncResult(object owner, AsyncCallback callback, object state)
        {
            this.owner = owner;
            this.callback = callback;
            this.state = state;
            threadId = Thread.CurrentThread.ManagedThreadId;
        }

        public void Signal()
        {
            isCompleted = true;

            completedSynchronously = threadId == Thread.CurrentThread.ManagedThreadId;

            waitHandle.Set();

            if (callback != null)
                callback(this);
        }

        public object Owner
        {
            get
            {
                return owner;
            }
        }

        public object AsyncState
        {
            get
            {
                return state;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                return waitHandle;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return completedSynchronously;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
        }
    }
}
