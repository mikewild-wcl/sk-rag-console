namespace SK.Rag.Application.Configuration;

public record AzureOpenAiOptions(
    string ApiKey,
    string Endpoint,
    string DeploymentName,
    string EmbeddingDeploymentName)
{
    public int Timeout { get; init; } = 30; // Default timeout in seconds
}