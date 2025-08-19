using Microsoft.Extensions.DependencyInjection;
using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders;

public class DocumentLoaderFactoryBuilder
{
    private IServiceProvider? _serviceProvider;

    public DocumentLoaderFactoryBuilder WithServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        return this;
    }

    public DocumentLoaderFactory Build()
    {
        var serviceProvider = _serviceProvider ?? DefaultServiceProvider;
        return new DocumentLoaderFactory(serviceProvider);
    }

    public static DocumentLoaderFactory CreateDefault() => new DocumentLoaderFactoryBuilder().Build();

    private static IServiceProvider DefaultServiceProvider
    {
        get
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddSingleton(_ => Mock.Of<IHtmlWebProvider>());
            services.AddSingleton<DocxDocumentLoader>();
            services.AddSingleton<MarkdownDocumentLoader>();
            services.AddSingleton<PdfDocumentLoader>();
            services.AddSingleton<TextDocumentLoader>();
            services.AddSingleton<WebsiteLoader>();
            return services.BuildServiceProvider();
        }
    }
}
