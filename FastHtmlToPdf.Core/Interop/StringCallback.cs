using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Core.Interop
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void StringCallback(IntPtr converter, [MarshalAs(UnmanagedType.LPStr)] string str);
}