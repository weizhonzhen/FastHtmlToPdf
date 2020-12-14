using System;
using System.Threading;

namespace FastHtmlToPdf.Threading
{
    internal class PostCompletedEventArgs : EventArgs
    {
        private SendOrPostCallback callback;

        private object state;

        private Exception error;

        public PostCompletedEventArgs(SendOrPostCallback callback, object state, Exception error)
        {
            this.callback = callback;
            this.state = state;
            this.error = error;
        }

        public SendOrPostCallback Callback
        {
            get
            {
                return callback;
            }
        }

        public object State
        {
            get
            {
                return state;
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
