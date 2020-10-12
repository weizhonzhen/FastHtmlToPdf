using FastHtmlToPdf.Context;

namespace FastHtmlToPdf.Model
{
    public class ImageDocument
    {
        public int MarginTop { get; set; }

        public int MarginLeft { get; set; }

        public int Height { get; set; } 

        public int Width { get; set; }

        public int Quality { get; set; } = 94;

        //public string Url { get; set; }

        public FormatEnum Format { get; set; } = FormatEnum.jpg;

        public int ScreenWidth { get; set; } = 800;

        public bool smartWidth { get; set; }

    }
}
