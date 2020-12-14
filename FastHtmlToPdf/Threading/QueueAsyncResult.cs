using System;

namespace FastHtmlToPdf.Threading
{
    internal class QueueAsyncResult : AsyncResult
    {
        private Delegate method;

        private object[] args;

        private object returnValue = null;

        private Exception error = null;

        private NotificationType notificationType;

        public QueueAsyncResult(object owner, Delegate method, object[] args,NotificationType notificationType) : base(owner, null, null)
        {
            this.method = method;
            this.args = args;
            this.notificationType = notificationType;
        }

        public void Invoke()
        {
            try
            {
                returnValue = method.DynamicInvoke(args);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                Signal();
            }
        }

        public object[] GetArgs()
        {
            return args;
        }

        public object ReturnValue
        {
            get
            {
                return returnValue;
            }
        }

        public Exception Error
        {
            get
            {
                return error;
            }
            set
            {
                error = value;
            }
        }

        public Delegate Method
        {
            get
            {
                return method;
            }
        }

        public NotificationType NotificationType
        {
            get
            {
                return notificationType;
            }
        }
    }

}
