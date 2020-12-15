using System;

namespace FastHtmlToPdf.Core.Threading
{
    internal class QueueAsyncResult : AsyncResult
    {
        private Delegate method;

        private object[] args;

        private object returnValue = null;

        private Exception error = null;

        public QueueAsyncResult(object owner, Delegate method, object[] args) : base(owner, null, null)
        {
            this.method = method;
            this.args = args;
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
    }
}
