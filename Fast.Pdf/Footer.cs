using static Fast.Pdf.PdfEnum;

namespace Fast.Pdf
{
    public class Footer
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
