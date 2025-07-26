namespace SK.Rag.Application.Configuration;

public record AzureOpenAIOptions(
    string ApiKey,
    string Endpoint,
    string DeploymentName,
    string EmbeddingDeploymentName)
{
    public const string SectionName = "AzureOpenAI";

    public int Timeout { get; init; } = 30; // Default timeout in seconds
}