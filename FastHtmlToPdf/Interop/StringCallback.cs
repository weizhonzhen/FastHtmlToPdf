using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void StringCallback(IntPtr converter, [MarshalAs(UnmanagedType.LPStr)] string str);
}