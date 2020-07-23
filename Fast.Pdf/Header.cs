﻿using static Fast.Pdf.PdfEnum;

namespace Fast.Pdf
{
    public class Header
    {
        public int FontSize { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public Type Type { get; set; }

        public Align Align { get; set; }
    }
}
