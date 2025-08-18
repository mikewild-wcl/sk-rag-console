using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders
{
    public static class DocumentServiceBuilder
    {
        public static DocumentService Build(
            IDocumentLoaderFactory? documentLoaderFactory = null,
            Kernel? kernel = null,
            ILogger<DocumentService>? logger = null) =>
            new(documentLoaderFactory ?? DocumentLoaderFactoryBuilder.Build(),
                kernel ?? new Kernel(),
                logger ?? new NullLogger<DocumentService>());
    }
}
