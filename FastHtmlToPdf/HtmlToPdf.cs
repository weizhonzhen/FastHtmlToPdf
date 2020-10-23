using FastHtmlToPdf.Model;
using System;

namespace FastHtmlToPdf
{
    public class HtmlToPdf
    {
        private Context.HtmlToPdf pdf;
        private AppDomain domain;

        public HtmlToPdf()
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.LoaderOptimization = LoaderOptimization.SingleDomain;
            domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
            var handle = Activator.CreateInstanceFrom(domain, typeof(Context.HtmlToPdf).Assembly.Location, typeof(Context.HtmlToPdf).FullName);
            pdf = handle.Unwrap() as Context.HtmlToPdf;
            if (AppDomain.CurrentDomain.IsDefaultAppDomain() == false)
                AppDomain.CurrentDomain.DomainUnload += DomainUnload;
        }

        public byte[] Convert(PdfDocument doc, string html)
        {
            return pdf.Convert(doc, html);
        }

        private void DomainUnload(object sender, EventArgs e)
        {
            pdf.Dispose();
            AppDomain.Unload(domain);          
        }
    }
}
