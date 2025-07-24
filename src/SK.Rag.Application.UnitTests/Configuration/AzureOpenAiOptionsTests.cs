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
        var options = new AzureOpenAiOptions(apiKey, endpoint, deploymentName, embeddingDeploymentName);

        // Assert
        options.ApiKey.Should().Be(apiKey);
        options.Endpoint.Should().Be(endpoint);
        options.DeploymentName.Should().Be(deploymentName);
        options.EmbeddingDeploymentName.Should().Be(embeddingDeploymentName);
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
        var options = AzureOpenAiOptionsBuilder.Build(
            apiKey,
            endpoint,
            deploymentName,
            embeddingDeploymentName,
            timeout);

        // Assert
        options.ApiKey.Should().Be(apiKey);
        options.Endpoint.Should().Be(endpoint);
        options.DeploymentName.Should().Be(deploymentName);
        options.EmbeddingDeploymentName.Should().Be(embeddingDeploymentName);
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

        var options = AzureOpenAiOptionsBuilder.Build();

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
        var options = new AzureOpenAiOptions("a", "b", "c", "d") { Timeout = 99 };

        // Assert
        options.Timeout.Should().Be(99);
    }
}
