using static FastPdf.PdfEnum;

namespace FastPdf
{
    public class PdfDocument
    {
        public bool DisplayHeader { get; set; }

        public bool DisplayFooter { get; set; }

        public Header Header { get; set; }

        public Footer Footer { get; set; }

        public int MarginTop { get; set; }

        public int MarginLeft { get; set; }

        public int MarginRight { get; set; }

        public int MarginBottom { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int DelayJavascript { get; set; } = 200;

        public Size Size { get; set; } = PdfEnum.Size.A4;
    }
}
