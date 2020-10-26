using FastHtmlToPdf.Core.Model;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace FastHtmlToPdf.Core
{
    public class HtmlToImage : AssemblyLoadContext
    {
        private Context.HtmlToImage pdf;
        private AssemblyLoadContext context;
        private Assembly assembly;
        public HtmlToImage()
        {
            assembly = Assembly.Load("FastHtmlToPdf.Core");
            context = GetLoadContext(assembly);
            context.Unloading += Context_Unloading;
        }

        private void Context_Unloading(AssemblyLoadContext obj)
        {
            pdf.Dispose();
            assembly = null;
            context = null;
        }

        public byte[] Convert(ImageDocument doc, string html)
        {
            foreach (var info in assembly.GetTypes().ToList())
            {
                if (info.FullName == "FastHtmlToPdf.Core.Context.HtmlToImage")
                {
                    pdf = Activator.CreateInstance(info) as Context.HtmlToImage;
                    return pdf.Convert(doc, html);
                }
            }
            throw new Exception("Convert error");
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
