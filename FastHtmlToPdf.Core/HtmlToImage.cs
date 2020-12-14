using FastHtmlToPdf.Core.Model;
using FastHtmlToPdf.Core.Threading;
using System;

namespace FastHtmlToPdf.Core
{
    public class HtmlToImage
	{
		private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
		private Context.HtmlToImage pdf = null;

		public HtmlToImage()
		{
			queue.Invoke((Action)(() => pdf = new Context.HtmlToImage()));
		}

		public byte[] Convert(ImageDocument doc, string inputHtml)
		{
			return (byte[])queue.Invoke((Func<string, byte[]>)((x) => pdf.Convert(doc, x)), inputHtml);
		}
	}
}
