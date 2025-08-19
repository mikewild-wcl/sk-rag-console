using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Services;
using SK.Rag.Application.UnitTests.Builders;

namespace SK.Rag.Application.UnitTests.Services;

public class SearchServiceTests
{
    private readonly SearchService _searchService;
    private readonly Mock<ILogger<SearchService>> _mockLogger;

    public SearchServiceTests()
    {
        var services = new ServiceCollection();
        services.AddInMemoryVectorStore();
        var kernel = new Kernel(services.BuildServiceProvider());
        _mockLogger = new Mock<ILogger<SearchService>>();
        _searchService = new SearchServiceBuilder()
            .WithKernel(kernel)
            .WithLogger(_mockLogger.Object)
            .Build();
    }

    [Fact]
    public async Task SemanticSearch_WithNullText_ShouldReturnEmptyList()
    {
        // Act
        var results = await _searchService.SemanticSearch(null);

        // Assert
        results.Should().BeEmpty();
    }

    [Fact]
    public async Task SemanticSearch_WithEmptyText_ShouldReturnEmptyList()
    {
        // Act
        var results = await _searchService.SemanticSearch("");

        // Assert
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public async Task SemanticSearch_WithDifferentMaxResults_ShouldRespectLimit(int maxResults)
    {
        // Arrange
        var text = "test search";

        // Act
        var results = await _searchService.SemanticSearch(text, maxResults);

        // Assert
        results.Should().NotBeNull();
        results.Count().Should().BeLessOrEqualTo(maxResults);
    }
}