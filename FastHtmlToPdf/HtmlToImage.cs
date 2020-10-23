using FastHtmlToPdf.Model;
using System;

namespace FastHtmlToPdf
{
    public class HtmlToImage
    {
        private Context.HtmlToImage pdf;
        private AppDomain domain;

        public HtmlToImage()
        {
            var setup = AppDomain.CurrentDomain.SetupInformation;
            setup.LoaderOptimization = LoaderOptimization.SingleDomain;
            domain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
            var handle = Activator.CreateInstanceFrom(domain, typeof(Context.HtmlToImage).Assembly.Location, typeof(Context.HtmlToImage).FullName);
            pdf = handle.Unwrap() as Context.HtmlToImage;
            if (AppDomain.CurrentDomain.IsDefaultAppDomain() == false)
                AppDomain.CurrentDomain.DomainUnload += DomainUnload;
        }

        public byte[] Convert(ImageDocument doc, string html)
        {
            return pdf.Convert(doc, html);
        }

        private void DomainUnload(object sender, EventArgs e)
        {
            AppDomain.Unload(domain);
            pdf.Dispose();
        }
    }
}
