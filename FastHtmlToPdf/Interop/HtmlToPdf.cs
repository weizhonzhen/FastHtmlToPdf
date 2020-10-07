using System;
using System.Runtime.InteropServices;

namespace FastHtmlToPdf.Interop
{
    internal static class HtmlToPdf
    {
        #region Create
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_init(int useGraphics);

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_global_settings();

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_converter(IntPtr globalSettings);
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr wkhtmltopdf_create_object_settings();
        #endregion

        #region object and global settings
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_set_object_setting(IntPtr settings,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] String name,
            [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] String value);

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_set_global_setting(IntPtr settings,
           [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] String name,
           [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] String value);
        #endregion

        #region Convert
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_set_error_callback(IntPtr converter, [MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_add_object(IntPtr converter, IntPtr objectSettings,
           [MarshalAs(UnmanagedType.CustomMarshaler, MarshalTypeRef = typeof(Utf8Marshaler))] String data);

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_convert(IntPtr converter);
        #endregion

        #region Dispose
        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern void wkhtmltopdf_destroy_converter(IntPtr converter);

        [DllImport("wkhtmltox.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
        public static extern int wkhtmltopdf_deinit();
        #endregion
    }
}
