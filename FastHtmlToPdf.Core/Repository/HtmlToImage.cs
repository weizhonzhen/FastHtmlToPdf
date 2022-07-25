﻿using FastHtmlToPdf.Core.Model;
using System;
using FastHtmlToPdf.Core.Threading;

namespace FastHtmlToPdf.Core.Repository
{
    public class HtmlToImage : IHtmlToImage
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
			#if NETCOREAPP
			converter.Dispose();
			#endif
		}
	}
}
