using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Prompts;
using SK.Rag.Application.Services.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;

namespace SK.Rag.Application.DocumentLoaders;

public class ChatService(
    Kernel kernel,
    ILogger<ChatService> logger) : IChatService
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<ChatService> _logger = logger;
    private readonly ChatHistory _chatHistory = [];

    public async Task<string> Chat(string prompt, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Chat called with prompt '{Prompt}'", prompt);

        return "This is a mock response to the prompt.";
    }

    public async IAsyncEnumerable<string> GetResponseAsync(
        string userMessage,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chatCompletionService = _kernel.GetRequiredService<IChatCompletionService>();

        if (!_chatHistory.Any())
        {
            _chatHistory.AddSystemMessage(SystemPrompts.GenAlphaSystemPrompt);
        }

        _chatHistory.AddUserMessage(userMessage);

        var promptExecutionSettings = chatCompletionService.BuildAzureOpenAIPromptExecutionSettings(0.9f);

        var responses = new StringBuilder();
        await foreach (var item in chatCompletionService.GetStreamingChatMessageContentsAsync(_chatHistory, promptExecutionSettings, cancellationToken: cancellationToken))
        {
            if(item is null)
            {
                _logger.LogWarning("Received null item in streaming response.");
                continue;
            }

            responses.Append(item.Content);
            yield return item.Content;
        }

        _chatHistory.AddAssistantMessage(responses.ToString());
    }
}
