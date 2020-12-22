using System;

namespace FastHtmlToPdf.Model
{
    public class Footer: MarshalByRefObject
    {
        public int FontSize { get; set; }

        public string Url { get; set; }

        public string Html { get; set; }

        public int Spacing { get; set; }

        public string Center { get; set; }

        public string Left { get; set; }

        public string Right { get; set; }

        public bool Line { get; set; }

        public string FontName { get; set; }
    }
}
