using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;

namespace SK.Rag.Application.UnitTests.Builders;

public static class SearchServiceBuilder
{
    public static SearchService Build(
        Kernel? kernel = null,
        ILogger<SearchService>? logger = null) =>
        new(
            kernel ?? new Kernel(),
            logger ?? new NullLogger<SearchService>());
}