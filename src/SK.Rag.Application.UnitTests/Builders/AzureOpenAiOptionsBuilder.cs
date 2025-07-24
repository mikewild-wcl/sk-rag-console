using SK.Rag.Application.Configuration;

namespace SK.Rag.Application.UnitTests.Builders;

public static class AzureOpenAiOptionsBuilder
{
    private const string DefaultApiKey = "TEST_API_KEY";
    private const string DefaultEndpoint = "https://openai.openai.azure.com/ https://test.connect.tlevels.gov.uk/";
    private const string DefaultDeploymentName = "test_deployment";
    private const string DefaultEmbeddingDeploymentName = "test_embedding_deployment";
    private const int DefaultTimeout = 30;

    public static AzureOpenAiOptions Build(
        string apiKey = DefaultApiKey,
        string endpoint = DefaultEndpoint,
        string deploymentName = DefaultDeploymentName,
        string embeddingDeploymentName = DefaultEmbeddingDeploymentName,
        int timeout = DefaultTimeout) => new(
            ApiKey: apiKey,
            Endpoint: endpoint,
            DeploymentName: deploymentName,
            EmbeddingDeploymentName: embeddingDeploymentName)
        { 
            Timeout = timeout,
        };
}
