using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.DocumentLoaders;

public class ChatService(
    Kernel kernel,
    ILogger<ChatService> logger) : IChatService
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<ChatService> _logger = logger;

    public async Task<string> Chat(string prompt)
    {
        _logger.LogInformation("Chat called with prompt '{Prompt}'", prompt);

        return "This is a mock response to the prompt.";
    }
}
