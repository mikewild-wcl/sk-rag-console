using SK.Rag.Application.Configuration;

namespace SK.Rag.Application.Extensions;

public static class AzureOpenAiOptionsExtensions
{
    public static string? GetModelIdOrDeploymentName(this AzureOpenAIOptions? options)
    {
        if (options is null)
        {
            return null;
        }

        var modelId = options.ModelId is { Length: > 0 }
            ? options.ModelId
            : options.DeploymentName;

        return modelId;
    }

    public static string? GetEmbeddingModelIdOrDeploymentName(this AzureOpenAIOptions? options)
    {
        if (options is null)
        {
            return null;
        }

        var modelId = options.EmbeddingModelId is { Length: > 0 }
            ? options.EmbeddingModelId
            : options.EmbeddingDeploymentName;

        return modelId;
    }
}
