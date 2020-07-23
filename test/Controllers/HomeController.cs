using Microsoft.AspNetCore.Mvc;
using Fast.Pdf;

namespace test.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var doc = new PdfDocument();
            doc.DelayJavascript = 500;

            doc.DisplayFooter = true;
            doc.Footer = new Footer();
            doc.Footer.Type = PdfEnum.Type.Url;
            doc.Footer.Url = "http://localhost:52711/home/footer";

            doc.DisplayHeader = true;
            doc.Header = new Header();
            doc.Header.Type = PdfEnum.Type.Url;
            doc.Header.Url = "http://localhost:52711/home/header";

           var bytes= FastPdf.ConvertHtmlString("<body><button>测试</botton></body>", doc, "test");

            return File(bytes, "application/pdf");
        }

        public IActionResult Footer()
        {
            return View();
        }

        public IActionResult Header()
        {
            return View();
        }
    }
}
