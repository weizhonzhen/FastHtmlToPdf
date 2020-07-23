using FastUntility.Core.Base;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Fast.Pdf
{
    public static class FastPdf
    {
        public static byte[] ConvertHtmlString(string html, PdfDocument doc, string fileName,bool isSave=false)
        {
            if (string.IsNullOrEmpty(html))
                return null;

            var exe = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf";
            var path = string.Format("{0}wwwroot\\fastpdf", System.AppDomain.CurrentDomain.BaseDirectory);
            var arguments = new StringBuilder();

            if (doc.MarginBottom != 0)
                arguments.AppendFormat("-B {0} ", doc.MarginBottom);

            if (doc.MarginLeft != 0)
                arguments.AppendFormat("-L {0} ", doc.MarginLeft);

            if (doc.MarginRight != 0)
                arguments.AppendFormat("-R {0} ", doc.MarginRight);

            if (doc.MarginTop != 0)
                arguments.AppendFormat("-T {0} ", doc.MarginTop);

            arguments.AppendFormat("-s {0} ", doc.Size);

            if (doc.Width != 0)
                arguments.AppendFormat("--page-width {0} ", doc.Width);

            if (doc.Height != 0)
                arguments.AppendFormat("--page-height {0} ", doc.Height);

            if (doc.DelayJavascript > 200)
                arguments.AppendFormat("--javascript-delay {0} ", doc.DelayJavascript);

            if (doc.DisplayHeader && doc.Header.Type == PdfEnum.Type.Url)
            {
                arguments.AppendFormat("--header-html {0} ", doc.Header.Url);
            }
            else if(doc.DisplayHeader && doc.Header.Type == PdfEnum.Type.Text)
            {
                arguments.AppendFormat("--header-{0} {1} ", doc.Header.Align.ToStr().ToLower(), doc.Header.Content.Replace(" ",""));

                if (doc.Header.FontSize != 0)
                    arguments.AppendFormat("--header-font-size {0} ", doc.Header.FontSize);
            }

            if (doc.DisplayFooter && doc.Footer.Type == PdfEnum.Type.Url)
            {
                arguments.AppendFormat("--footer-html {0} ", doc.Footer.Url);
            }
            else if (doc.DisplayFooter && doc.Footer.Type == PdfEnum.Type.Text)
            {
                arguments.AppendFormat("--footer-{0} {1} ", doc.Footer.Align.ToStr().ToLower(), doc.Footer.Content.Replace(" ", ""));

                if (doc.Footer.FontSize != 0)
                    arguments.AppendFormat("--footer-font-size {0} ", doc.Footer.FontSize);
            }

            arguments.Append(" - -");

            var proc = new Process();
            proc.StartInfo = new ProcessStartInfo();
            proc.StartInfo.FileName = Path.Combine(path, exe);
            proc.StartInfo.Arguments = arguments.ToString();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.WorkingDirectory = path;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();

            using (var input = proc.StandardInput)
            {
                input.WriteLine(HtmlEncode(html));
            }

            using (var ms = new MemoryStream())
            {
                using (var stream = proc.StandardOutput.BaseStream)
                {
                    var buffer = new byte[4096];
                    int read;

                    while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                }

                if (ms.Length == 0)
                {
                    BaseLog.SaveLog(proc.StandardError.ReadToEnd(), "HtmlToPdf");
                    return null;
                }
                else
                {
                    proc.WaitForExit();
                    var bytes = ms.ToArray();
                    if (isSave)
                    {
                        using (var fs = new FileStream(string.Format("{0}{1}.pdf", AppDomain.CurrentDomain.BaseDirectory, fileName), FileMode.OpenOrCreate))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                            fs.Close();
                        }
                    }

                    return bytes;
                }
            }
        }

        public static byte[] ConvertHtmUrl(string url, PdfDocument doc, string fileName, bool isSave = false)
        {
            return ConvertHtmlString(BaseUrl.GetUrl(url), doc, fileName);
        }

        private static string HtmlEncode(string html)
        {
            var chars = html.ToCharArray();
            var sb = new StringBuilder(html.Length + (int)(html.Length * 0.1));
            foreach (var c in chars)
            {
                var value = Convert.ToInt32(c);
                if (value > 127)
                    sb.AppendFormat("&#{0};", value);
                else
                    sb.Append(c);
            }
            return sb.ToString();
        }
    }
}
