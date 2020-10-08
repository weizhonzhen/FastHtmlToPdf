﻿using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Fast.Pdf
{
    public static class FastPdf
    {
        public static byte[] ConvertHtmlString(string html, PdfDocument doc)
        {
            try
            {
                if (string.IsNullOrEmpty(html))
                    return null;
                if (!File.Exists(FullPath))
                    Create(Content);

                using (var proc = new Process())
                {
                    proc.StartInfo = new ProcessStartInfo();
                    proc.StartInfo.FileName = FullPath;
                    proc.StartInfo.Arguments = Arguments(doc);
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.WorkingDirectory = FilesDirectory;
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
                            return null;
                        else
                        {
                            proc.WaitForExit();
                            return ms.ToArray();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.StackTrace + ex.Message);
            }
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

        private static string Arguments(PdfDocument doc)
        {
            var arguments = new StringBuilder();
            arguments.Append("--print-media-type ");

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

                if (doc.Header.Spacing != 0)
                    arguments.AppendFormat("--header-spacing {0} ", doc.Header.Spacing);

                if (doc.Header.Line)
                    arguments.Append("--header-line  ");
            }
            else if (doc.DisplayHeader && doc.Header.Type == PdfEnum.Type.Text)
            {
                arguments.AppendFormat("--header-{0} {1} ", doc.Header.Align.ToString().ToLower(), doc.Header.Content.Replace(" ", ""));

                if (doc.Header.Spacing != 0)
                    arguments.AppendFormat("--header-spacing {0} ", doc.Header.Spacing);

                if (doc.Header.FontSize != 0)
                    arguments.AppendFormat("--header-font-size {0} ", doc.Header.FontSize);

                if (doc.Header.Line)
                    arguments.Append("--header-line  ");
            }

            if (doc.DisplayFooter && doc.Footer.Type == PdfEnum.Type.Url)
            {
                arguments.AppendFormat("--footer-html {0} ", doc.Footer.Url);

                if (doc.Footer.Spacing != 0)
                    arguments.AppendFormat("--footer-spacing {0} ", doc.Footer.Spacing);

                if (doc.Footer.Line)
                    arguments.Append("--footer-line  ");
            }
            else if (doc.DisplayFooter && doc.Footer.Type == PdfEnum.Type.Text)
            {
                arguments.AppendFormat("--footer-{0} {1} ", doc.Footer.Align.ToString().ToLower(), doc.Footer.Content.Replace(" ", ""));

                if (doc.Footer.Spacing != 0)
                    arguments.AppendFormat("--footer-spacing {0} ", doc.Footer.Spacing);

                if (doc.Footer.FontSize != 0)
                    arguments.AppendFormat("--footer-font-size {0} ", doc.Footer.FontSize);

                if (doc.Footer.Line)
                    arguments.Append("--footer-line  ");
            }

            arguments.Append("- -");

            return arguments.ToString();
        }

        private static void Create(byte[] fileContent)
        {
            try
            {
                if (!Directory.Exists(FilesDirectory))
                    Directory.CreateDirectory(FilesDirectory);

                using (var file = File.Open(FullPath, FileMode.Create))
                {
                    file.Write(fileContent, 0, fileContent.Length);
                }
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string FilesDirectory
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), "FastPdf", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            }
        }

        private static string FullPath
        {
            get
            {
                return Path.Combine(FilesDirectory, RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "wkhtmltopdf.exe" : "wkhtmltopdf");
            }
        }

        private static byte[] Content
        {
            get
            {
                var Resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("Fast.Pdf.Assets.wkhtmltopdf.zip");
                using (var zip = new ZipArchive(Resource))
                {
                    var entry = zip.Entries.ToList().Find(a => a.FullName == "wkhtmltopdf.exe");
                    using (var stream = entry.Open())
                    {
                        var content = new byte[entry.Length];
                        stream.Read(content, 0, (int)entry.Length);
                        return content;
                    }
                }
            }
        }
    }
}
