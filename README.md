# Fast.Pdf
Fast.pdf create pdf by wkhtmltopdf
      

nuget url: https://www.nuget.org/packages/Fast.Data.Core/

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
           bytes= FastPdf.ConvertHtmUrl("www.a.com", doc, "test",true);
