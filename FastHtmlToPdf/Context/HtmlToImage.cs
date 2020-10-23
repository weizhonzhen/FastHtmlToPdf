using FastHtmlToPdf.Assets;
using FastHtmlToPdf.Interop;
using FastHtmlToPdf.Model;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FastHtmlToPdf.Context
{
    internal class HtmlToImage : MarshalByRefObject, IDisposable
    {
        private IntPtr GlobalSettings;
        private IntPtr Converter;
        private ZipFile zip;

        public HtmlToImage()
        {
            zip = new ZipFile();
            Interop.HtmlToImage.wkhtmltoimage_init(0);
            GlobalSettings = Interop.HtmlToImage.wkhtmltoimage_create_global_settings();
        }
        public void Dispose()
        {
            if (Converter != IntPtr.Zero)
                Interop.HtmlToImage.wkhtmltoimage_destroy_converter(Converter);

            Interop.HtmlToImage.wkhtmltoimage_deinit();
            zip.Dispose();
            zip = null;
            GlobalSettings = IntPtr.Zero;
            Converter = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        public byte[] Convert(ImageDocument doc, string html)
        {
            if (doc == null)
                throw new Exception("Fast.HtmlToImage ImageDocument is not null");

            #region setting
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "web.defaultEncoding", "utf-8");
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "web.loadImages", "true");
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "web.enableJavascript", "true");
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "load.jsdelay", "1000");
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "load.loadErrorHandling", "skip");
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "load.debugJavascript", "true");

            if (doc.Width != 0)
                Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "screenWidth", doc.Width.ToString());

            if (doc.Height != 0)
                Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "screenHeight", doc.Height.ToString());

            //if (doc.MarginTop != 0)
            //    Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "crop.top", doc.MarginTop.ToString());

            //if (doc.MarginLeft != 0)
            //    Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "crop.left", doc.MarginLeft.ToString());

            //Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "in", doc.Url);
            Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "fmt", doc.Format.ToString());
            if (doc.SmartWidth)
                Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "smartWidth", "true");

            if ((doc.Format == FormatEnum.png || doc.Format == FormatEnum.svg) && doc.Transparent)
                Interop.HtmlToImage.wkhtmltoimage_set_global_setting(GlobalSettings, "transparent", "true");

            #endregion

            StringCallback errorCallback = (converter, errorText) =>
            {
                throw new Exception(errorText);
            };

            Converter = Interop.HtmlToImage.wkhtmltoimage_create_converter(GlobalSettings, Encoding.UTF8.GetBytes(html));
            Interop.HtmlToImage.wkhtmltoimage_set_error_callback(Converter, errorCallback);

            if (Interop.HtmlToImage.wkhtmltoimage_convert(Converter) != 0)
            {
                IntPtr tmp;
                var len = Interop.HtmlToImage.wkhtmltoimage_get_output(Converter, out tmp);
                var result = new byte[len];
                Marshal.Copy(tmp, result, 0, result.Length);
                return result;
            }
            else
                throw new Exception("FastHtmlToImage error");
        }
    }
}
