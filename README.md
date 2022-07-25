# FastHtmlToPdf
FastHtmlToPdf create pdf by wkhtmltox
      
nuget url: https://www.nuget.org/packages/Fast.HtmlToPdf for net framework 

nuget url: https://www.nuget.org/packages/Fast.HtmlToPdf.core for net core 
          
           services.AddFastHtmlToImage();
           services.AddFastHtmlToPdf();
                    
          IHtmlToPdf htmlToPdf;
          IHtmlToImage htmlToImage;       
                   
                    
           using(var pdf = new HtmlToPdf())
           {
                var path = string.Format("{0}\\print.htm", System.AppDomain.CurrentDomain.BaseDirectory);
                var html = System.IO.File.ReadAllText(path);
                var doc = new PdfDocument();
                doc.MarginBottom = 50;
                doc.MarginTop = 50;
            
                doc.DisplayFooter = true;
                doc.Footer.Url = "www.a.com";
                doc.Footer.Spacing = 10;

                doc.DisplayHeader = true;
                doc.Header.Url = "www.a.com"; 
                
                //by html header footer
                doc.Host = HttpContext.Request.Host.Value;(net core)                
                doc.Host = HttpContext.Request.Url;(NET Framework)                
                doc.Footer.Html="<b>test</b>";                
                doc.Header.Html="<b>test</b>";
                
                doc.Header.Spacing = 10;
                                             
                htmlToPdf.Convert(doc, html);
                return File(pdf.Convert(doc, html), "application/pdf");
            }
            
            page html must be include  "<!DOCTYPE html>"

            //html to image
             using (var img = new HtmlToImage())
            {
                var path = string.Format("{0}\\print.htm", System.AppDomain.CurrentDomain.BaseDirectory);
                var html = System.IO.File.ReadAllText(path);

                 var doc = new ImageDocument();
                doc.Width = 1000;
                doc.Height = 800;
                doc.Format = FormatEnum.png;


                htmlToImage.Convert(doc, html);
                var bytes = img.Convert(doc, html);
                return File(bytes, "image/png");
            }


# FastPdf
FastPdf create pdf by wkhtmltopdf
      
nuget url: https://www.nuget.org/packages/Fast.Pdf for net framework 

nuget url: https://www.nuget.org/packages/Fast.Pdf for net core 

          using(var pdf = new FastPdf())
          {
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
                        
            return File(pdf.Convert(doc,html), "application/pdf");
          }
            page html must be include  "<!DOCTYPE html>"
