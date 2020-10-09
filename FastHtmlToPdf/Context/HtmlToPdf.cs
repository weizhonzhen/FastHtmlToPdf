﻿using FastHtmlToPdf.Assets;
using FastHtmlToPdf.Interop;
using FastHtmlToPdf.Model;
using System;
using System.IO;

namespace FastHtmlToPdf
{
    public class HtmlToPdf : IDisposable
    {
        private int UseX11Graphics = 0;
        private IntPtr GlobalSettings;
        private IntPtr Converter;
        private ZipFile zip;
        private IntPtr ObjectSettings;

        public HtmlToPdf()
        {
            zip = new ZipFile();
            Interop.HtmlToPdf.wkhtmltopdf_init(UseX11Graphics);
            GlobalSettings = Interop.HtmlToPdf.wkhtmltopdf_create_global_settings();
            Converter = Interop.HtmlToPdf.wkhtmltopdf_create_converter(GlobalSettings);
            ObjectSettings = Interop.HtmlToPdf.wkhtmltopdf_create_object_settings();
        }

        public void Dispose()
        {
            if (Converter != IntPtr.Zero)
                Interop.HtmlToPdf.wkhtmltopdf_destroy_converter(Converter);

            GlobalSettings = IntPtr.Zero;
            Converter = IntPtr.Zero;
            ObjectSettings = IntPtr.Zero;
            Interop.HtmlToPdf.wkhtmltopdf_deinit();
            zip.Dispose();
            zip = null;
        }

        public byte[] Convert(PdfDocument doc, string html)
        {
            var FilesDirectory = string.Format("{0}HtmlToPdf", Path.GetTempPath());
            if (!Directory.Exists(FilesDirectory))
                Directory.CreateDirectory(FilesDirectory);
            var fileName = string.Format("{0}\\{1}.pdf", FilesDirectory, Guid.NewGuid().ToString());
            if (doc == null)
                throw new Exception("Fast.HtmlToPdf PdfDocument is not null");

            #region object set
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.defaultEncoding", "utf-8");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.loadImages", "true");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "web.enableJavascript", "true");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.jsdelay", "1000");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.loadErrorHandling", "skip");
            Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "load.debugJavascript", "true");

            if (doc.DisplayHeader)
            {
                if (doc.Header.FontSize != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.fontSize", doc.Header.FontSize.ToString());
                if (doc.Header.Spacing != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.spacing", doc.Header.Spacing.ToString());
                if (!string.IsNullOrEmpty(doc.Header.Url))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "header.htmlUrl", doc.Header.Url);
            }

            if (doc.DisplayFooter)
            {
                if (doc.Footer.FontSize != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.fontSize", doc.Footer.FontSize.ToString());
                if (doc.Footer.Spacing != 0)
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.spacing", doc.Footer.Spacing.ToString());
                if (!string.IsNullOrEmpty(doc.Footer.Url))
                    Interop.HtmlToPdf.wkhtmltopdf_set_object_setting(ObjectSettings, "footer.htmlUrl", doc.Footer.Url);
            }
            #endregion

            #region global set
            if (!string.IsNullOrEmpty(doc.Size))
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.pageSize", doc.Size);
            else
                Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "size.pageSize", "A4");

            Interop.HtmlToPdf.wkhtmltopdf_set_global_setting(GlobalSettings, "out", fileName);

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
            Interop.HtmlToPdf.wkhtmltopdf_add_object(Converter, ObjectSettings, html);
            Interop.HtmlToPdf.wkhtmltopdf_convert(Converter);

            if (File.Exists(fileName))
            {
                var result = File.ReadAllBytes(fileName);
                File.Delete(fileName);
                return result;
            }
            else
                throw new Exception("Fast.HtmlToPdf create pdf Failed");
        }
    }
}