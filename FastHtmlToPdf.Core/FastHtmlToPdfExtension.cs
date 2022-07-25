using FastHtmlToPdf.Core.Assets;
using FastHtmlToPdf.Core.Repository;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class FastHtmlToPdfExtension
    {
        public static IServiceCollection AddFastHtmlToPdf(this IServiceCollection serviceCollection)
        {
            new ZipFile();
            serviceCollection.AddScoped<IHtmlToPdf, HtmlToPdf>();
            return serviceCollection;
        }

        public static IServiceCollection AddFastHtmlToImage(this IServiceCollection serviceCollection)
        {
            new ZipFile();
            serviceCollection.AddScoped<IHtmlToImage, HtmlToImage>();
            return serviceCollection;
        }
    }
}
