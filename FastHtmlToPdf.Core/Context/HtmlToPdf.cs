using FastHtmlToPdf.Core.Assets;
using FastHtmlToPdf.Core.Interop;
using FastHtmlToPdf.Core.Model;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FastHtmlToPdf.Core.Context
{
    internal class HtmlToPdf : MarshalByRefObject, IDisposable
    {
        private IntPtr GlobalSettings;
        private IntPtr Converter;
        private ZipFile zip;
        private IntPtr ObjectSettings;

        public HtmlToPdf()
        {
            zip = new ZipFile();
            Interop.HtmlToPdf.wkhtmltopdf_init(0);
            GlobalSettings = Interop.HtmlToPdf.wkhtmltopdf_create_global_settings();
            Converter = Interop.HtmlToPdf.wkhtmltopdf_create_converter(GlobalSettings);
            ObjectSettings = Interop.HtmlToPdf.wkhtmltopdf_create_object_settings();
        }

        public void Dispose()
        {
            if (Converter != IntPtr.Zero)
                Interop.HtmlToPdf.wkhtmltopdf_destroy_converter(Converter);
            Interop.HtmlToPdf.wkhtmltopdf_destroy_global_settings(GlobalSettings);
            Interop.HtmlToPdf.wkhtmltopdf_destroy_object_settings(ObjectSettings);
            Interop.HtmlToPdf.wkhtmltopdf_deinit();
            zip.Dispose();
            zip = null;
            GlobalSettings = IntPtr.Zero;
            Converter = IntPtr.Zero;
            ObjectSettings = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        public byte[] Convert(PdfDocument doc, string html)
        {
            if (doc == null)
                throw new Exception("Fast.HtmlToPdf PdfDocument is not null");

            var headerId = Guid.NewGuid().ToString();
            var footerId = Guid.NewGuid().ToString();

            if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\wwwroot"))
                Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\wwwroot");
                
            var headerPath = string.Format("{0}\\wwwroot\\{1}.html", Directory.GetCurrentDirectory(), headerId);
            var footerPath = string.Format("{0}\\wwwroot\\{1}.html", Directory.GetCurrentDirectory(), footerId);

            #region object set
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.defaultEncoding", "utf-8");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.loadImages", "true");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.enableJavascript", "true");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.jsdelay", "1000");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.loadErrorHandling", "skip");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.debugJavascript", "true");

            if (doc.DisplayHeader)
            {
                if (!string.IsNullOrEmpty(doc.Header.Html) && !string.IsNullOrEmpty(doc.Host))
                {
                    using (var fs = File.Create(headerPath))
                    {
                        using (var writer = new StreamWriter(fs, Encoding.UTF8))
                        {
                            if (doc.Header.Html.Contains("<!DOCTYPE html>"))
                                writer.Write(doc.Header.Html);
                            else
                                writer.Write(string.Format("<!DOCTYPE html>{0}", doc.Header.Html));
                        }
                    }

                    if (!doc.Host.Contains("http://"))
                        doc.Host = "http://" + doc.Host;

                    if (doc.Host.Substring(doc.Host.Length - 1, 1) == "/")
                        doc.Header.Url = string.Format("{0}{1}.html", doc.Host, headerId);
                    else
                        doc.Header.Url = string.Format("{0}/{1}.html", doc.Host, headerId);
                }

                if (doc.Header.FontSize != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.fontSize", doc.Header.FontSize.ToString());
                if (doc.Header.Spacing != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.spacing", doc.Header.Spacing.ToString());
                if (!string.IsNullOrEmpty(doc.Header.Url))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.htmlUrl", doc.Header.Url);
                if (!string.IsNullOrEmpty(doc.Header.Center))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.center", doc.Header.Center);
                if (!string.IsNullOrEmpty(doc.Header.Left))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.left", doc.Header.Left);
                if (!string.IsNullOrEmpty(doc.Header.Right))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.right", doc.Header.Right);
                if (doc.Header.Line)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.line", "true");
                if (!string.IsNullOrEmpty(doc.Header.FontName))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.fontName", doc.Header.FontName);
            }

            if (doc.DisplayFooter)
            {
                if (!string.IsNullOrEmpty(doc.Footer.Html) && !string.IsNullOrEmpty(doc.Host))
                {
                    using (var fs = File.Create(footerPath))
                    {
                        using (var writer = new StreamWriter(fs, Encoding.UTF8))
                        {
                            if (doc.Footer.Html.Contains("<!DOCTYPE html>"))
                                writer.Write(doc.Footer.Html);
                            else
                                writer.Write(string.Format("<!DOCTYPE html>{0}", doc.Footer.Html));
                        }
                    }

                    if (doc.Host.Substring(doc.Host.Length - 1, 1) == "/")
                        doc.Footer.Url = string.Format("{0}{1}.html", doc.Host, footerId);
                    else
                        doc.Footer.Url = string.Format("{0}/{1}.html", doc.Host, footerId);
                }

                if (doc.Footer.FontSize != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.fontSize", doc.Footer.FontSize.ToString());
                if (doc.Footer.Spacing != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.spacing", doc.Footer.Spacing.ToString());
                if (!string.IsNullOrEmpty(doc.Footer.Url))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.htmlUrl", doc.Footer.Url);
                if (!string.IsNullOrEmpty(doc.Footer.Center))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.center", doc.Footer.Center);
                if (!string.IsNullOrEmpty(doc.Footer.Left))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.left", doc.Footer.Left);
                if (!string.IsNullOrEmpty(doc.Footer.Right))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.right", doc.Footer.Right);
                if (doc.Footer.Line)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.line", "true");
                if (!string.IsNullOrEmpty(doc.Footer.FontName))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.fontName", doc.Footer.FontName);
            }
            #endregion

            #region global set
            if (!string.IsNullOrEmpty(doc.Size))
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.pageSize", doc.Size);
            else
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.pageSize", "A4");

            if (doc.Width != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.width", doc.Width * 0.04 + "cm");

            if (doc.Height != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.height", doc.Height * 0.04 + "cm");

            if (doc.MarginTop != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "margin.top", doc.MarginTop * 0.04 + "cm");

            if (doc.MarginBottom != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "margin.bottom", doc.MarginBottom * 0.04 + "cm");

            if (doc.MarginLeft != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "margin.left", doc.MarginLeft * 0.04 + "cm");

            if (doc.MarginRight != 0)
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "margin.right", doc.MarginRight * 0.04 + "cm");
            #endregion

            StringCallback errorCallback = (converter, errorText) =>
            {
                throw new Exception(errorText);
            };

            Interop.HtmlToPdf.wkhtmltopdf_set_error_callback(Converter, errorCallback);
            Interop.HtmlToPdf.wkhtmltopdf_add_object(Converter, ObjectSettings, Encoding.UTF8.GetBytes(html));

            if (Interop.HtmlToPdf.wkhtmltopdf_convert(Converter) != 0)
            {
                IntPtr tmp;
                var len = Interop.HtmlToPdf.wkhtmltopdf_get_output(Converter, out tmp);
                var result = new byte[len];
                Marshal.Copy(tmp, result, 0, result.Length);
                tmp = IntPtr.Zero;
                File.Delete(headerPath);
                File.Delete(footerPath);
                return result;
            }
            else
                throw new Exception("FastHtmlToPdf error");
        }
    }
}