using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FastHtmlToPdf.Core.Interop
{
    internal class Utf8Marshaler : ICustomMarshaler
    {
        public static Utf8Marshaler _staticInstance;

        public IntPtr MarshalManagedToNative(object ManagedObj)
        {
            if (ManagedObj == null)
                return IntPtr.Zero;
            if (!(ManagedObj is string))
                throw new MarshalDirectiveException("UTF8Marshaler must be string.");

            byte[] strbuf = Encoding.UTF8.GetBytes((string)ManagedObj);
            IntPtr buffer = Marshal.AllocHGlobal(strbuf.Length + 1);
            Marshal.Copy(strbuf, 0, buffer, strbuf.Length);

            Marshal.WriteByte(new IntPtr(buffer.ToInt64() + (long)strbuf.Length), 0);
            return buffer;
        }

        public unsafe object MarshalNativeToManaged(IntPtr pNativeData)
        {
            byte* walk = (byte*)pNativeData;

            while (*walk != 0)
            {
                walk++;
            }
            int length = (int)(walk - (byte*)pNativeData);
            byte[] strbuf = new byte[length - 1];
            Marshal.Copy(pNativeData, strbuf, 0, length - 1);
            string data = Encoding.UTF8.GetString(strbuf);
            return data;
        }

        public void CleanUpNativeData(IntPtr pNativeData)
        {
            Marshal.FreeHGlobal(pNativeData);
        }

        public void CleanUpManagedData(object ManagedObj)
        {
        }

        public int GetNativeDataSize()
        {
            return -1;
        }

        public static ICustomMarshaler GetInstance(string param)
        {
            if (_staticInstance == null)
                return _staticInstance = new Utf8Marshaler();
            return _staticInstance;
        }
    }
}
