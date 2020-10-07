# Fast.Pdf
Fast.pdf create pdf by wkhtmltopdf
      

nuget url: https://www.nuget.org/packages/FastHtmlToPdf/

            var doc = new PdfDocument();
            
            doc.DisplayFooter = true;
            doc.Footer.Url = "http://localhost:52711/home/footer";

            doc.DisplayHeader = true;
            doc.Header.Url = "http://localhost:52711/home/header";
            
            using(var pdf = HtmlToPdf.Instance())
            {
                 var bytes = pdf.Convert(doc , "<body><button>测试</botton></body>");
            }
