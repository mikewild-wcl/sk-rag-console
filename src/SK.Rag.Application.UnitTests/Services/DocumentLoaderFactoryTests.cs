using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services;
using SK.Rag.Application.UnitTests.Builders;

namespace SK.Rag.Application.UnitTests.Services;

public class DocumentLoaderFactoryTests
{
    private readonly DocumentLoaderFactory _factory;

    public DocumentLoaderFactoryTests()
    {
        _factory = DocumentLoaderFactoryBuilder.Build();
    }

    [Theory]
    [InlineData(DocumentType.Docx, typeof(DocxDocumentLoader))]
    [InlineData(DocumentType.Markdown, typeof(MarkdownDocumentLoader))]
    [InlineData(DocumentType.Pdf, typeof(PdfDocumentLoader))]
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