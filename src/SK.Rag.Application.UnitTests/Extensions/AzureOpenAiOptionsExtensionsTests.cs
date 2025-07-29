using SK.Rag.Application.Configuration;
using SK.Rag.Application.Extensions;

namespace SK.Rag.Application.UnitTests.Extensions;

public class AzureOpenAiOptionsExtensionsTests
{
    [Fact]
    public void GetModelIdOrDeploymentName_ReturnsNull_WhenOptionsIsNull()
    {
        AzureOpenAIOptions? options = null;
        options.GetModelIdOrDeploymentName().Should().BeNull();
    }

    [Fact]
    public void GetModelIdOrDeploymentName_ReturnsModelId_WhenModelIdIsNotEmpty()
    {
        var options = new AzureOpenAIOptions("key", "endpoint", "deployment", "embeddingDeployment")
        {
            ModelId = "gpt-4"
        };
        options.GetModelIdOrDeploymentName().Should().Be("gpt-4");
    }

    [Fact]
    public void GetModelIdOrDeploymentName_ReturnsDeploymentName_WhenModelIdIsNullOrEmpty()
    {
        var options = new AzureOpenAIOptions("key", "endpoint", "deployment", "embeddingDeployment")
        {
            ModelId = ""
        };
        options.GetModelIdOrDeploymentName().Should().Be("deployment");
    }

    [Fact]
    public void GetEmbeddingModelIdOrDeploymentName_ReturnsNull_WhenOptionsIsNull()
    {
        AzureOpenAIOptions? options = null;
        options.GetEmbeddingModelIdOrDeploymentName().Should().BeNull();
    }

    [Fact]
    public void GetEmbeddingModelIdOrDeploymentName_ReturnsEmbeddingModelId_WhenNotEmpty()
    {
        var options = new AzureOpenAIOptions("key", "endpoint", "deployment", "embeddingDeployment")
        {
            EmbeddingModelId = "embed-ada"
        };
        options.GetEmbeddingModelIdOrDeploymentName().Should().Be("embed-ada");
    }

    [Fact]
    public void GetEmbeddingModelIdOrDeploymentName_ReturnsEmbeddingDeploymentName_WhenEmbeddingModelIdIsNullOrEmpty()
    {
        var options = new AzureOpenAIOptions("key", "endpoint", "deployment", "embeddingDeployment")
        {
            EmbeddingModelId = ""
        };
        options.GetEmbeddingModelIdOrDeploymentName().Should().Be("embeddingDeployment");
    }
}