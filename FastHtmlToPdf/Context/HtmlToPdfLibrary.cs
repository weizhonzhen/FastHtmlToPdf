using FastHtmlToPdf.Assets;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace FastHtmlToPdf
{
    internal class HtmlToPdfLibrary
    {
        private const string LibraryFilename = "wkhtmltox.dll";
        private const string LibraryFilename32 = "wkhtmltox_32.dll";
        private const string LibraryFilename64 = "wkhtmltox_64.dll";

        public static NativeLibrary Load()
        {
            return NativeLibrary.Load(LibraryFilename, LoadLibraryContent);
        }

        private static byte[] LoadLibraryContent()
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                throw new Exception(String.Format("Platform {0} is not supported", Platform));

            using (var zip = new ZipArchive(Resource))
            {
               var entry =  zip.Entries.ToList().Find(a => a.FullName == FileName);
                using (var stream = entry.Open())
                {
                    var content = new byte[entry.Length];
                    stream.Read(content, 0, (int)entry.Length);
                    return content;
                }
            }
        }

        private static Stream Resource
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetManifestResourceStream("FastHtmlToPdf.Assets.wkhtmltox.zip");
            }
        }

        private static string FileName
        {
            get
            {
                return Environment.Is64BitProcess ? LibraryFilename64 : LibraryFilename32;
            }
        }

        private static string Platform
        {
            get
            {
                return Enum.GetName(typeof(PlatformID), Environment.OSVersion.Platform);
            }
        }
    }
}
