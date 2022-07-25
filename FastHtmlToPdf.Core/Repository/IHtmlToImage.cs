
using FastHtmlToPdf.Core.Model;

namespace FastHtmlToPdf.Core.Repository
{
    public interface IHtmlToImage
    {
        byte[] Convert(ImageDocument doc, string inputHtml);

        void Dispose();
    }
}
