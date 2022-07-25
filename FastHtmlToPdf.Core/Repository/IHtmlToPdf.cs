using FastHtmlToPdf.Core.Model;

namespace FastHtmlToPdf.Core.Repository
{
    public interface IHtmlToPdf
    {
        byte[] Convert(PdfDocument doc, string inputHtml);

        void Dispose();
    }
}
