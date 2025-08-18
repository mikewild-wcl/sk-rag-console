using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.UnitTests.Builders;

public static class ChatServiceBuilder
{
    public static ChatService Build(
        Kernel? kernel = null,
        ISearchService searchService = null,
        ILogger<ChatService>? logger = null) =>
        new(kernel ?? new Kernel(),
            searchService ?? SearchServiceBuilder.Build(kernel ?? new Kernel()),
            logger ?? new NullLogger<ChatService>());
}
