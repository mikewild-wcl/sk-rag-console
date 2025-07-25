using Microsoft.Extensions.DependencyInjection;
using SK.Rag.Application.Models;

namespace SK.Rag.Application.DocumentLoaders.Interfaces;
public class DocumentLoaderFactory(
    IServiceProvider serviceProvider): IDocumentLoaderFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public IDocumentLoader Create(DocumentType documentType) =>
        documentType switch
        {
            DocumentType.Pdf => _serviceProvider.GetRequiredService<PdfDocumentLoader>(),
            DocumentType.Docx => _serviceProvider.GetRequiredService<DocxDocumentLoader>(),
            DocumentType.Text => _serviceProvider.GetRequiredService<TextDocumentLoader>(),
            DocumentType.WebPage => _serviceProvider.GetRequiredService<WebsiteLoader>(),
            _ => throw new ArgumentException("Invalid document type", nameof(documentType))
        };
}
