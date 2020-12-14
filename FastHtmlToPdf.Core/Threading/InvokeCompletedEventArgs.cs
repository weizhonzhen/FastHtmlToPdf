using System;

namespace FastHtmlToPdf.Core.Threading
{
    internal class InvokeCompletedEventArgs : EventArgs
    {
        private Delegate method;

        private object[] args;

        private object result;

        private Exception error;

        public InvokeCompletedEventArgs(Delegate method, object[] args, object result, Exception error)
        {
            this.method = method;
            this.args = args;
            this.result = result;
            this.error = error;
        }

        public object[] GetArgs()
        {
            return args;
        }

        public Delegate Method
        {
            get
            {
                return method;
            }
        }

        public object Result
        {
            get
            {
                return result;
            }
        }

        public Exception Error
        {
            get
            {
                return error;
            }
        }
    }
}
