using Microsoft.SemanticKernel.Services;
using SK.Rag.Application.Extensions;

namespace SK.Rag.Application.UnitTests.Extensions;

public class SemanticKernelExtensionsTests
{
    [Fact]
    public void IsReasoningModel_ShouldReturnTrue_ForO1Model()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "ModelId", "o1" }
        });

        var result = mockService.Object.IsReasoningModel();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsReasoningModel_ShouldReturnTrue_ForO3MiniModel()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "ModelId", "o3-mini" }
        });
        var result = mockService.Object.IsReasoningModel();

        result.Should().BeTrue();
    }

    [Fact]
    public void IsReasoningModel_ShouldReturnFalse_ForNonOxModelId()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "ModelId", "gpt-4" }
        });

        var result = mockService.Object.IsReasoningModel();

        result.Should().BeFalse();
    }
       

    [Fact]
    public void IsReasoningModel_ShouldCheckDeploymentName_WhenModelIdAndModelIdAttributeAreNull()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "DeploymentName", "o7-mini" }
        });

        var result = mockService.Object.IsReasoningModel();

        result.Should().BeTrue();
    }

    [Fact]
    public void BuildAzureOpenAIPromptExecutionSettings_ShouldSetTemperature_WhenNotReasoningModel()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "ModelId", "gpt-4" }
        });

        var settings = mockService.Object.BuildAzureOpenAIPromptExecutionSettings(0.55f);

        settings.Temperature.Should().BeApproximately(0.55f, 0.005f);
    }

    [Fact]
    public void BuildAzureOpenAIPromptExecutionSettings_ShouldNotSetTemperature_ForReasoningModel()
    {
        var mockService = new Mock<IAIService>();
        mockService.SetupGet(s => s.Attributes).Returns(new Dictionary<string, object?>
        {
            { "ModelId", "o4-mini" }
        });

        var settings = mockService.Object.BuildAzureOpenAIPromptExecutionSettings(0.99f);

        settings.Temperature.Should().BeNull();
    }
}