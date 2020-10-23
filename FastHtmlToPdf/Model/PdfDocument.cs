using System;

namespace FastHtmlToPdf.Model
{
    public class PdfDocument : MarshalByRefObject
    {
        public bool DisplayHeader { get; set; }

        public bool DisplayFooter { get; set; }

        public Header Header { get; set; } = new Header();

        public Footer Footer { get; set; } = new Footer();

        public int MarginTop { get; set; }

        public int MarginLeft { get; set; }

        public int MarginRight { get; set; }

        public int MarginBottom { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public string Size { get; set; } = "A4";
    }
}
