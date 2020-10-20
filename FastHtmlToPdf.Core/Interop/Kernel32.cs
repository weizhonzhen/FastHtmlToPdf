using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Core.Interop
{
    internal static class Kernel32
    {
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool FreeLibrary(IntPtr hModule);

        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string filename);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool SetDllDirectory(string PathName);
    }
}