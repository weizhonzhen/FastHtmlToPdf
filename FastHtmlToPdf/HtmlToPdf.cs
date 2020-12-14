using FastHtmlToPdf.Model;
using FastHtmlToPdf.Threading;
using System;

namespace FastHtmlToPdf
{
	public class HtmlToPdf
	{
		private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
		private static Context.HtmlToPdf pdf = null;
		private Context.HtmlToPdf converter = null;

		public HtmlToPdf()
		{
			lock (queue)
			{
				if (pdf == null)
				{
					queue.Invoke((Action)(() => pdf = new Context.HtmlToPdf()));

					AppDomain.CurrentDomain.ProcessExit += (o, e) =>
						queue.Invoke((Action)(() => {
							pdf.Dispose();
							pdf = null;
						}));
				}
			}

			queue.Invoke((Action)(() => converter = new Context.HtmlToPdf()));
		}

		public byte[] Convert(PdfDocument doc, string inputHtml)
		{
			return (byte[])queue.Invoke((Func<string, byte[]>)((x) => converter.Convert(doc, x)), inputHtml);
		}
	}
}