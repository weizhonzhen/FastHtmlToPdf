using FastHtmlToPdf.Interop;
using System;

namespace FastHtmlToPdf.Assets
{
    internal class NativeLibrary
    {
        private readonly Lazy<ZipFile> LibFile;
        private IntPtr LibHandle;

        private NativeLibrary(string libraryFilename, Func<byte[]> libraryContentProvider)
        {
            LibFile = new Lazy<ZipFile>(() => ZipFile.Instance(libraryFilename, libraryContentProvider));
        }

        public static NativeLibrary Load(string libraryFilename, Func<byte[]> libraryContentProvider)
        {
            var lib = new NativeLibrary(libraryFilename, libraryContentProvider);
            lib.LoadLibrary();
            return lib;
        }

        public void Dispose()
        {
            if (LibHandle != IntPtr.Zero)
                FreeLibrary();
        }

        private void FreeLibrary()
        {
            Kernel32.FreeLibrary(LibHandle);
            LibHandle = IntPtr.Zero;
        }

        private void LoadLibrary()
        {
            LibHandle = Kernel32.LoadLibrary(LibFile.Value.FullPath);
            if (LibHandle == IntPtr.Zero)
                throw new Exception(string.Format("FastHtmlToPdf Failed to load {0}", LibFile.Value.FullPath));
        }
    }
}