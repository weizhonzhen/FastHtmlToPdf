using FastHtmlToPdf.Core.Model;
using FastHtmlToPdf.Core.Threading;
using System;

namespace FastHtmlToPdf.Core
{
    public class HtmlToImage
	{
		private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
		private static Context.HtmlToImage pdf = null;
		private Context.HtmlToImage converter = null;

		public HtmlToImage()
		{
			lock (queue)
			{
				if (pdf == null)
				{
					queue.Invoke((Action)(() => pdf = new Context.HtmlToImage()));

					AppDomain.CurrentDomain.ProcessExit += (o, e) =>
						queue.Invoke((Action)(() => {
							pdf.Dispose();
							pdf = null;
						}));
				}
			}

			queue.Invoke((Action)(() => converter = new Context.HtmlToImage()));
		}

		public byte[] Convert(ImageDocument doc, string inputHtml)
		{
			return (byte[])queue.Invoke((Func<string, byte[]>)((x) => converter.Convert(doc, x)), inputHtml);
		}
	}
}
