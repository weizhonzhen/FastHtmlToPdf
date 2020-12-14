using FastHtmlToPdf.Model;
using FastHtmlToPdf.Threading;
using System;

namespace FastHtmlToPdf
{
    public class HtmlToPdf
    {
        private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
		private Context.HtmlToPdf pdf = null;

		public HtmlToPdf()
		{
			queue.Invoke((Action)(() => pdf = new Context.HtmlToPdf()));
		}

		public byte[] Convert(PdfDocument doc,string inputHtml)
		{
			return (byte[])queue.Invoke((Func<string, byte[]>)((x) => pdf.Convert(doc, x)), inputHtml);
		}
	}
}