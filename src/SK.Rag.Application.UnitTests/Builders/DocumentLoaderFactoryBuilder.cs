using Microsoft.Extensions.DependencyInjection;
using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders
{
    public static class DocumentLoaderFactoryBuilder
    {
        public static DocumentLoaderFactory Build(
            IServiceProvider? serviceProvider = null)
        {
            if (serviceProvider is null)
            {
                var services = new ServiceCollection();

                services.AddLogging();
                services.AddSingleton(_ => Mock.Of<IHtmlWebProvider>());
                services.AddSingleton<DocxDocumentLoader>();
                services.AddSingleton<MarkdownDocumentLoader>();
                services.AddSingleton<PdfDocumentLoader>();
                services.AddSingleton<TextDocumentLoader>();
                services.AddSingleton<WebsiteLoader>();

                serviceProvider = services.BuildServiceProvider();
            }

            return new DocumentLoaderFactory(serviceProvider);
        }
    }
}
