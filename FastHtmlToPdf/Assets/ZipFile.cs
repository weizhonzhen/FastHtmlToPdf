using System;
using System.IO;
using System.Reflection;

namespace FastHtmlToPdf.Assets
{
    internal class ZipFile
    {
        private readonly string Filename;
        private readonly Func<byte[]> FileContent;

        private ZipFile(string _Filename, Func<byte[]> _FileContent)
        {
            Filename = _Filename;
            FileContent = _FileContent;
        }

        public static ZipFile Instance(string Filename, Func<byte[]> FileContent)
        {
            var zipFile = new ZipFile(Filename, FileContent);
            zipFile.Create();
            return zipFile;
        }

        private void Create()
        {
            if (!File.Exists(FullPath))
                Create(FileContent());
        }

        private void Create(byte[] fileContent)
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

        public string FullPath 
        {
            get
            {
                return Path.Combine(FilesDirectory, Filename);
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
    }
}