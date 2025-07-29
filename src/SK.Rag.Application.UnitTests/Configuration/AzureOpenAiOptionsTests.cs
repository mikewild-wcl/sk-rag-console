using SK.Rag.Application.Configuration;
using SK.Rag.Application.UnitTests.Builders;

namespace SK.Rag.Application.UnitTests.Configuration;

public class AzureOpenAiOptionsTests
{
    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var apiKey = "key";
        var endpoint = "endpoint";
        var deploymentName = "deployment";
        var embeddingDeploymentName = "embedding";

        // Act
        var options = new AzureOpenAIOptions(apiKey, endpoint, deploymentName, embeddingDeploymentName);

        // Assert
        options.ApiKey.Should().Be(apiKey);
        options.Endpoint.Should().Be(endpoint);
        options.DeploymentName.Should().Be(deploymentName);
        options.ModelId.Should().BeNull();
        options.EmbeddingDeploymentName.Should().Be(embeddingDeploymentName);
        options.EmbeddingModelId.Should().BeNull();
        options.Timeout.Should().Be(30);
    }

    [Fact]
    public void Constructor_ShouldSetPropertiesCorrectly_WhenUsingBuilder()
    {
        // Arrange
        var apiKey = "key";
        var endpoint = "endpoint";
        var deploymentName = "deployment";
        var embeddingDeploymentName = "embedding";
        var timeout = 1001;

        // Act
        var options = AzureOpenAIOptionsBuilder.Build(
            apiKey,
            endpoint,
            deploymentName,
            embeddingDeploymentName: embeddingDeploymentName,
            timeout: timeout);

        // Assert
        options.ApiKey.Should().Be(apiKey);
        options.Endpoint.Should().Be(endpoint);
        options.DeploymentName.Should().Be(deploymentName);
        options.EmbeddingDeploymentName.Should().Be(embeddingDeploymentName);
        options.ModelId.Should().BeNull();
        options.EmbeddingModelId.Should().BeNull();
        options.Timeout.Should().Be(1001);
    }

    [Fact]
    public void With_ShouldSetPropertiesCorrectly()
    {
        //Arrange
        var apiKey = "key";
        var endpoint = "endpoint";
        var deploymentName = "deployment";
        var embeddingDeploymentName = "embedding";
        var timeout = 100;

        var options = AzureOpenAIOptionsBuilder.Build();

        // Act
        options = options with
        {
            ApiKey = apiKey,
            Endpoint = endpoint,
            DeploymentName = deploymentName,
            EmbeddingDeploymentName = embeddingDeploymentName,
            Timeout = timeout
        };

        //Assert
        options.ApiKey.Should().Be(apiKey);
        options.Endpoint.Should().Be(endpoint);
        options.DeploymentName.Should().Be(deploymentName);
        options.EmbeddingDeploymentName.Should().Be(embeddingDeploymentName);
        options.Timeout.Should().Be(100); 
    }

    [Fact]
    public void Timeout_CanBeSetViaInit()
    {
        // Arrange
        var options = new AzureOpenAIOptions("a", "b", "c", "d") { Timeout = 99 };

        // Assert
        options.Timeout.Should().Be(99);
    }
}
