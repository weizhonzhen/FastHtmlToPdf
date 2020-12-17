using Fast.Pdf.Threading;
using System;

namespace Fast.Pdf
{
    public class FastPdf : IDisposable
    {
        private static readonly FastPdfQueue queue = new FastPdfQueue();
        private Context.FastPdf converter = null;

        public FastPdf()
        {
            queue.Invoke((Action)(() => converter = new Context.FastPdf()));
        }

        public byte[] Convert(PdfDocument doc, string inputHtml)
        {
            return (byte[])queue.Invoke((Func<string, byte[]>)((x) => converter.Convert(x,doc)), inputHtml);
        }

        public void Dispose()
        {
            converter.Dispose();
        }
    }
}
