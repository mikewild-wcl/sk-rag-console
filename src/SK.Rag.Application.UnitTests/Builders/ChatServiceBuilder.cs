using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.SemanticKernel;
using SK.Rag.Application.DocumentLoaders;

namespace SK.Rag.Application.UnitTests.Builders;

public static class ChatServiceBuilder
{
    public static ChatService Build(
        Kernel? kernel = null,
        ILogger<ChatService>? logger = null) =>
        new(// kernel ?? new Kernel(),
            logger ?? new NullLogger<ChatService>());
}
