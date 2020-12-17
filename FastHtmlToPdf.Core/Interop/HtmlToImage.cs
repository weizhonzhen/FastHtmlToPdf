using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Core.Interop
{
    internal class HtmlToImage
    {
        #region create
        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern int wkhtmltoimage_init(int useGraphics);

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern IntPtr wkhtmltoimage_create_global_settings();

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern int wkhtmltoimage_convert(IntPtr converter);
        #endregion

        #region object and global settings
        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern void wkhtmltoimage_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern int wkhtmltoimage_set_global_setting(IntPtr settings, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] string name, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] string value);
        #endregion

        #region Convert
        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern IntPtr wkhtmltoimage_create_converter(IntPtr globalSettings, byte[] data);

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern int wkhtmltoimage_get_output(IntPtr converter, out IntPtr data);

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern void wkhtmltoimage_set_finished_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);
        #endregion

        #region Dispose
        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern void wkhtmltoimage_destroy_converter(IntPtr converter);

        #if NETCOREAPP
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        #else
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        #endif
        public static extern int wkhtmltoimage_deinit();
        #endregion
    }
}