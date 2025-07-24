using Microsoft.Extensions.Logging;
using SK.Rag.CommandLine.ConsoleApp.Services.Interfaces;

namespace SK.Rag.CommandLine.ConsoleApp.Services;

public class ChatService(
    ILogger<ChatService> logger) : IChatService
{
    private readonly ILogger<ChatService> _logger = logger;

    public async Task<string> ChatAsync(string prompt)
    {
        _logger.LogInformation("Chat called with prompt '{Prompt}'", prompt);

        return "This is a mock response to the prompt.";
    }
}
