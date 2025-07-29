using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Services;

namespace SK.Rag.Application.Extensions;

public static class SemanticKernelExtensions
{
    public static AzureOpenAIPromptExecutionSettings BuildAzureOpenAIPromptExecutionSettings(this IAIService aiService, float temperature = 0.7f)
    {
        var promptExecutionSettings = new AzureOpenAIPromptExecutionSettings();

        if (!aiService.IsReasoningModel())
        {
            // o<n> models do not support temperature - https://github.com/ai-christianson/RA.Aid/issues/70
            promptExecutionSettings.Temperature = temperature;
        }

        return promptExecutionSettings;
    }

    public static bool IsReasoningModel(this IAIService aiService)
    {
        var modelId = aiService.GetModelId(); // This uses ModelId attribute if available
        modelId ??= aiService.Attributes.TryGetValue("DeploymentName", out var deploymentNameAttribute)
            ? deploymentNameAttribute?.ToString() 
            : null;

        return modelId is not null && modelId.StartsWith('o');
    }
}
