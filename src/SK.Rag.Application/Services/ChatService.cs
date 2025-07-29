using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Services;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Services.Interfaces;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SK.Rag.Application.DocumentLoaders;

public class ChatService(
    Kernel kernel,
    ILogger<ChatService> logger) : IChatService
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<ChatService> _logger = logger;
    private readonly ChatHistory _chatHistory = [];

    private const string BoomerSystemPrompt = """
        You are a helpful Silicon Valley pirate. 
        Answer the user's question as a boomer tech bro from a pirate family.
        ---
        """;

    private const string GenAlphaSystemPrompt = """
        You are a helpful Silicon Valley pirate. 
        Answer the user's question as a gen-alpha tech bro from a pirate family.
        ---
        """;

    private const string GenZPirateSystemPrompt = """
        You are a helpful Silicon Valley pirate. 
        Answer the user's question as a gen-z tech bro from a pirate family.
        ---
        """;

    private const string MillennialPirateSystemPrompt = """
        You are a helpful Silicon Valley pirate. 
        Answer the user's question as a millennial tech bro from a pirate family.
        ---
        """;

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
            _chatHistory.AddSystemMessage(BoomerSystemPrompt);
        }

        _chatHistory.AddUserMessage(userMessage);

        //var promptExecutionSettings = GetPromptExecutionSettings(chatCompletionService);
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

    //TODO: Can rewrite as an extension method on IAIService
    private AzureOpenAIPromptExecutionSettings GetPromptExecutionSettings(IAIService service)
    {
        var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings();

        var modelName = service.GetModelId();
        var attributes = service.Attributes;
        foreach (var attribute in attributes)
        {
            _logger.LogInformation("ChatCompletionService Attribute: {Key} = {Value}", attribute.Key, attribute.Value);
        }

        var hasDeployment = service.Attributes.TryGetValue("DeploymentName", out var deployment);
        var hasModelId = service.Attributes.TryGetValue("ModelId", out var modelId);

        var isMini = hasModelId && Regex.IsMatch(modelId.ToString(), "o[\\d]-mini");

        if (!isMini)
        {
            promptExecutionSettings.Temperature = 0.9f;
        }

        return promptExecutionSettings;
    }
}
