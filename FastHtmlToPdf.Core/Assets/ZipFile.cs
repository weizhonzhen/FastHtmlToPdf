using FastHtmlToPdf.Core.Interop;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace FastHtmlToPdf.Core.Assets
{
    internal class ZipFile
    {
        private string FileName = "wkhtmltox.dll";
        private string LibraryFilename32 = "wkhtmltox_32.dll";
        private string LibraryFilename64 = "wkhtmltox_64.dll";
        private IntPtr LibHandle;

        public ZipFile()
        {
            if (!File.Exists(FullPath))
                Create();

            (new DirectoryInfo(FilesDirectory)).GetFileSystemInfos().ToList().ForEach(a => {
                if (a is FileInfo && a.Name == FileName && ((FileInfo)a).Length == 0)
                    Create();
            });

            LibHandle = Kernel32.LoadLibrary(FullPath);
            if (LibHandle == IntPtr.Zero)
                throw new Exception(string.Format("FastHtmlToPdf Failed to load {0}", FullPath));
        }

        public void Dispose()
        {
            if (LibHandle != IntPtr.Zero)
            {
                Kernel32.FreeLibrary(LibHandle);
                Kernel32.FreeLibrary(LibHandle);
                LibHandle = IntPtr.Zero;
                GC.SuppressFinalize(this);
            }
        }

        private void Create()
        {
            try
            {
                if (!Directory.Exists(FilesDirectory))
                    Directory.CreateDirectory(FilesDirectory);

                using (var file = File.Open(FullPath, FileMode.Create))
                {
                    file.Write(Content, 0, Content.Length);
                }
            }
            catch (IOException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string FullPath 
        {
            get
            {
                return Path.Combine(FilesDirectory, FileName);
            }
        }

        private string FilesDirectory
        {
            get
            {
                return Path.Combine(Path.GetTempPath(), "FastHtmlToPdf", Version);
            }
        }

        private string Version
        {
            get
            {
                return string.Format("{0}_{1}", Assembly.GetExecutingAssembly().GetName().Version, Environment.Is64BitProcess ? 64 : 32);
            }
        }

        private byte[] Content
        {
            get
            {
                var platform = Enum.GetName(typeof(PlatformID), Environment.OSVersion.Platform);
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                    throw new Exception(String.Format("Platform {0} is not supported", platform));

                var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("FastHtmlToPdf.Core.Assets.wkhtmltox.zip");
                using (var zip = new ZipArchive(resource))
                {
                    var fileName = Environment.Is64BitProcess ? LibraryFilename64 : LibraryFilename32;
                    var entry = zip.Entries.ToList().Find(a => a.FullName == fileName);
                    using (var stream = entry.Open())
                    {
                        var content = new byte[entry.Length];
                        stream.Read(content, 0, (int)entry.Length);

                        resource.Dispose();
                        return content;
                    }
                }
            }
        }
    }
}