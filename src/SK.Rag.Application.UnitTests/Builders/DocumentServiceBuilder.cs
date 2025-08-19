using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders;

public class DocumentServiceBuilder
{
    private IDocumentLoaderFactory? _documentLoaderFactory;
    private Kernel? _kernel;
    private ILogger<DocumentService>? _logger;

    public DocumentServiceBuilder WithDocumentLoaderFactory(IDocumentLoaderFactory documentLoaderFactory)
    {
        _documentLoaderFactory = documentLoaderFactory;
        return this;
    }

    public DocumentServiceBuilder WithKernel(Kernel kernel)
    {
        _kernel = kernel;
        return this;
    }

    public DocumentServiceBuilder WithLogger(ILogger<DocumentService> logger)
    {
        _logger = logger;
        return this;
    }

    public DocumentService Build() =>
        new(
            _documentLoaderFactory ?? DocumentLoaderFactoryBuilder.CreateDefault(),
            _kernel ?? new Kernel(),
            _logger ?? new NullLogger<DocumentService>());

    public static DocumentService CreateDefault() => new DocumentServiceBuilder().Build();
}
