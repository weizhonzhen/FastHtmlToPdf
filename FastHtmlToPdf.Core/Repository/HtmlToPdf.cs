﻿using FastHtmlToPdf.Core.Model;
using FastHtmlToPdf.Core.Threading;
using System;

namespace FastHtmlToPdf.Core.Repository
{
    public class HtmlToPdf : IHtmlToPdf
    {
        private static readonly HtmlToPdfQueue queue = new HtmlToPdfQueue();
        private Context.HtmlToPdf converter = null;

        public HtmlToPdf()
        {
            queue.Invoke((Action)(() => converter = new Context.HtmlToPdf()));
        }

        public byte[] Convert(PdfDocument doc, string inputHtml)
        {
            return (byte[])queue.Invoke((Func<string, byte[]>)((x) => converter.Convert(doc, x)), inputHtml);
        }

        public void Dispose()
        {
            converter.Dispose();
        }
    }
}
