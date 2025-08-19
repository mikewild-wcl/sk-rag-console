using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.Application.UnitTests.Builders;

namespace SK.Rag.Application.UnitTests.Services;

public class ChatServiceTests
{
    private readonly Mock<ILogger<ChatService>> _mockLogger;
    private readonly ChatService _chatService;
    private readonly Mock<ISearchService> _mockSearchService;

    public ChatServiceTests()
    {
        var kernel = Kernel.CreateBuilder().Build();
        _mockLogger = new Mock<ILogger<ChatService>>();
        _mockSearchService = new Mock<ISearchService>();

        _chatService = new ChatServiceBuilder()
            .WithKernel(kernel)
            .WithSearchService(_mockSearchService.Object)
            .WithLogger(_mockLogger.Object)
            .Build();
    }

    [Fact]
    public async Task Chat_WithNestedSearchService_ValidPrompt_ShouldReturnResponse()
    {
        // Arrange
        var kernel = Kernel.CreateBuilder().Build();
        var chatService = new ChatServiceBuilder()
            .WithKernel(kernel)
            .WithSearchService(a => a.WithKernel(kernel))
            .WithLogger(_mockLogger.Object)
            .Build();
        var prompt = "Hello, how are you?";

        // Act
        var result = await chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithValidPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "Hello, how are you?";

        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithEmptyPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "";

        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithNullPrompt_ShouldReturnResponse()
    {
        // Act
        var result = await _chatService.Chat(null!, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithWhitespacePrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "   ";

        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithLongPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "This is a very long prompt that contains multiple sentences and should still work correctly. " +
                    "The service should handle long prompts gracefully and return the expected mock response.";

        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_WithSpecialCharacters_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "What about special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task Chat_ShouldLogInformation()
    {
        // Arrange
        var prompt = "test prompt";

        // Act
        await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Chat called with prompt '{prompt}'")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Chat_WithNullPrompt_ShouldLogInformation()
    {
        // Act
        await _chatService.Chat(null!, TestContext.Current.CancellationToken);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Chat called with prompt")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Chat_MultipleCalls_ShouldLogEachCall()
    {
        // Arrange
        var prompt1 = "first prompt";
        var prompt2 = "second prompt";

        // Act
        await _chatService.Chat(prompt1, TestContext.Current.CancellationToken);
        await _chatService.Chat(prompt2, TestContext.Current.CancellationToken);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Chat called with prompt")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("How are you today?")]
    [InlineData("Tell me a joke")]
    [InlineData("What is the weather like?")]
    public async Task Chat_WithVariousPrompts_ShouldReturnConsistentResponse(string prompt)
    {
        // Act
        var result = await _chatService.Chat(prompt, TestContext.Current.CancellationToken);

        // Assert
        result.Should().Be("This is a mock response to the prompt.");
    }
}
