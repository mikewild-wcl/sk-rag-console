using Microsoft.Extensions.DependencyInjection;
using SK.Rag.Application.DocumentLoaders;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.Services;

public class DocumentLoaderFactory(
    IServiceProvider serviceProvider) : IDocumentLoaderFactory
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
