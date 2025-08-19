using SK.Rag.Application.Configuration;

namespace SK.Rag.Application.UnitTests.Builders;

public class AzureOpenAIOptionsBuilder
{
    private string _apiKey = DefaultApiKey;
    private string _endpoint = DefaultEndpoint;
    private string _deploymentName = DefaultDeploymentName;
    private string _embeddingDeploymentName = DefaultEmbeddingDeploymentName;
    private string? _modelId;
    private string? _embeddingModelId;
    private int _timeout = DefaultTimeout;

    private const string DefaultApiKey = "TEST_API_KEY";
    private const string DefaultEndpoint = "https://openai.openai.azure.com/ https://test.connect.tlevels.gov.uk/";
    private const string DefaultDeploymentName = "test_deployment";
    private const string DefaultEmbeddingDeploymentName = "test_embedding_deployment";
    private const int DefaultTimeout = 30;

    public AzureOpenAIOptionsBuilder WithApiKey(string apiKey)
    {
        _apiKey = apiKey;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithEndpoint(string endpoint)
    {
        _endpoint = endpoint;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithDeploymentName(string deploymentName)
    {
        _deploymentName = deploymentName;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithEmbeddingDeploymentName(string embeddingDeploymentName)
    {
        _embeddingDeploymentName = embeddingDeploymentName;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithModelId(string? modelId)
    {
        _modelId = modelId;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithEmbeddingModelId(string? embeddingModelId)
    {
        _embeddingModelId = embeddingModelId;
        return this;
    }

    public AzureOpenAIOptionsBuilder WithTimeout(int timeout)
    {
        _timeout = timeout;
        return this;
    }

    public AzureOpenAIOptions Build() => new AzureOpenAIOptions(_apiKey, _endpoint, _deploymentName, _embeddingDeploymentName)
    {
        ModelId = _modelId,
        EmbeddingModelId = _embeddingModelId,
        Timeout = _timeout
    };

    public static AzureOpenAIOptions CreateDefault() => new AzureOpenAIOptionsBuilder().Build();
}
