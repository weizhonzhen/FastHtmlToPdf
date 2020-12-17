using static Fast.Pdf.Core.PdfEnum;

namespace Fast.Pdf.Core
{
    public class Header
    {
        public int FontSize { get; set; }

        public string Content { get; set; }

        public string Url { get; set; }

        public Type Type { get; set; }

        public Align Align { get; set; }

        public int Spacing { get; set; }

        public bool Line { get; set; }
    }
}
