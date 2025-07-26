using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SK.Rag.Application.Services;

namespace SK.Rag.Application.UnitTests.Builders
{
    public static class DocumentServiceBuilder
    {
        public static DocumentService Build(
            ILogger<DocumentService>? logger = null) =>
            new(logger ?? new NullLogger<DocumentService>());
    }
}
