using FastHtmlToPdf.Core.Model;
using FastHtmlToPdf.Core.Threading;
using System;

namespace FastHtmlToPdf.Core
{
    public class HtmlToImage : IDisposable
	{
		private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
		private Context.HtmlToImage converter = null;

		public HtmlToImage()
		{ 
			queue.Invoke((Action)(() => converter = new Context.HtmlToImage()));
		}

		public byte[] Convert(ImageDocument doc, string inputHtml)
		{
			return (byte[])queue.Invoke((Func<string, byte[]>)((x) => converter.Convert(doc, x)), inputHtml);
		}

		public void Dispose()
		{
			converter.Dispose();
		}
	}
}
