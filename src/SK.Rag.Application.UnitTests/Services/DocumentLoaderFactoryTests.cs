using Microsoft.Extensions.DependencyInjection;
using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Services;

public class DocumentLoaderFactoryTests
{
    private readonly ServiceProvider _serviceProvider;
    private readonly DocumentLoaderFactory _factory;

    public DocumentLoaderFactoryTests()
    {
        var services = new ServiceCollection();

        services.AddLogging();
        services.AddSingleton(_ => Mock.Of<IHtmlWebProvider>());
        services.AddSingleton<PdfDocumentLoader>();
        services.AddSingleton<DocxDocumentLoader>();
        services.AddSingleton<TextDocumentLoader>();
        services.AddSingleton<WebsiteLoader>();

        _serviceProvider = services.BuildServiceProvider();

        _factory = new DocumentLoaderFactory(_serviceProvider);
    }

    [Theory]
    [InlineData(DocumentType.Pdf, typeof(PdfDocumentLoader))]
    [InlineData(DocumentType.Docx, typeof(DocxDocumentLoader))]
    [InlineData(DocumentType.Text, typeof(TextDocumentLoader))]
    [InlineData(DocumentType.WebPage, typeof(WebsiteLoader))]
    public void Create_ShouldReturnCorrectLoaderType(DocumentType type, Type expectedType)
    {
        var loader = _factory.Create(type);
        loader.Should().NotBeNull();
        loader.Should().BeOfType(expectedType);
    }

    [Fact]
    public void Create_WithUnknownType_ShouldThrowArgumentException()
    {
        var act = () => _factory.Create(DocumentType.Unknown);
        act.Should().Throw<ArgumentException>().WithMessage("*Invalid document type*");
    }
}