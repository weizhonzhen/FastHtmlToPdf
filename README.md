# FastHtmlToPdf
FastHtmlToPdf create pdf by wkhtmltox
      
nuget url: https://www.nuget.org/packages/Fast.HtmlToPdf
          
                    
           using(var pdf = new HtmlToPdf())
           {
                var path = string.Format("{0}\\print.htm", System.AppDomain.CurrentDomain.BaseDirectory);
                var html = System.IO.File.ReadAllText(path);
                var doc = new PdfDocument();
                doc.MarginBottom = 50;
                doc.MarginTop = 50;
            
                doc.DisplayFooter = true;
                doc.Footer.Url = "www.a.com"; //in FastHtmltoPdf, but not in FastHtmltoPdf.Core
                doc.Footer.Spacing = 10;

                doc.DisplayHeader = true;
                doc.Header.Url = "www.a.com"; //in FastHtmltoPdf, but not in FastHtmltoPdf.Core
                doc.Header.Spacing = 10;
                                                       
                return File(pdf.Convert(doc, html), "application/pdf");
            }
            
            page header html must be include  "<!DOCTYPE html>"

            //html to image
             using (var img = new HtmlToImage())
            {
                var path = string.Format("{0}\\print.htm", System.AppDomain.CurrentDomain.BaseDirectory);
                var html = System.IO.File.ReadAllText(path);

                 var doc = new ImageDocument();
                doc.Width = 1000;
                doc.Height = 800;
                doc.Format = FormatEnum.png;

                var bytes = img.Convert(doc, html);
                return File(bytes, "image/png");
            }


# FastPdf
FastPdf create pdf by wkhtmltopdf
      
nuget url: https://www.nuget.org/packages/Fast.Pdf

            var path = string.Format("{0}\\print.htm", System.AppDomain.CurrentDomain.BaseDirectory);
            var html = System.IO.File.ReadAllText(path);
            var doc = new PdfDocument();
            
            doc.DisplayFooter = true;
            doc.Footer.Type = PdfEnum.Type.Url;
            doc.Footer.Url = "http://" + HttpContext.Request.Host.Value + "/home/footer";
           
            doc.DisplayHeader = true;
            doc.Header.Type = PdfEnum.Type.Url;
            doc.Header.Url = "http://" + HttpContext.Request.Host.Value + "/home/header";
                                           
            doc.MarginBottom = 50;
            doc.MarginTop = 50;
                        
            return File(FastPdf.ConvertHtmlString(html,doc), "application/pdf");
                        
            page html Header must be include  "<!DOCTYPE html>"
