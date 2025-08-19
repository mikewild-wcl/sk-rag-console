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
    public async Task _WithValidPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "Hello, how are you?";

        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _WithEmptyPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "";

        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _WithNullPrompt_ShouldReturnResponse()
    {
        // Act
        var result = await _chatService.Chat(null!);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _WithWhitespacePrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "   ";

        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _WithLongPrompt_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "This is a very long prompt that contains multiple sentences and should still work correctly. " +
                    "The service should handle long prompts gracefully and return the expected mock response.";

        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _WithSpecialCharacters_ShouldReturnResponse()
    {
        // Arrange
        var prompt = "What about special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Be("This is a mock response to the prompt.");
    }

    [Fact]
    public async Task _ShouldLogInformation()
    {
        // Arrange
        var prompt = "test prompt";

        // Act
        await _chatService.Chat(prompt);

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
    public async Task _WithNullPrompt_ShouldLogInformation()
    {
        // Act
        await _chatService.Chat(null!);

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
    public async Task _MultipleCalls_ShouldLogEachCall()
    {
        // Arrange
        var prompt1 = "first prompt";
        var prompt2 = "second prompt";

        // Act
        await _chatService.Chat(prompt1);
        await _chatService.Chat(prompt2);

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
    public async Task _WithVariousPrompts_ShouldReturnConsistentResponse(string prompt)
    {
        // Act
        var result = await _chatService.Chat(prompt);

        // Assert
        result.Should().Be("This is a mock response to the prompt.");
    }
}
