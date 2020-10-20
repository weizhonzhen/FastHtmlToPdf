using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void IntCallback(IntPtr converter, int str);
}
