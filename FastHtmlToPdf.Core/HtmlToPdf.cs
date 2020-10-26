using FastHtmlToPdf.Core.Model;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace FastHtmlToPdf.Core
{
    public class HtmlToPdf : AssemblyLoadContext
    {
        private AssemblyLoadContext context;
        private Assembly assembly;
        public HtmlToPdf()
        {
            assembly = Assembly.Load("FastHtmlToPdf.Core");
            context = GetLoadContext(assembly);
            context.Unloading += Context_Unloading;
        }

        private void Context_Unloading(AssemblyLoadContext obj)
        {
            assembly = null;
            context = null;
        }

        public byte[] Convert(PdfDocument doc, string html)
        {
            foreach (var info in assembly.GetTypes().ToList())
            {
                if (info.FullName == "FastHtmlToPdf.Core.Context.HtmlToPdf")
                {
                    var pdf = Activator.CreateInstance(info) as Context.HtmlToPdf;
                    return pdf.Convert(doc, html);
                }
            }
            return null;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
