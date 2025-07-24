using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services;
using SK.Rag.Application.UnitTests.Builders;

namespace SK.Rag.Application.UnitTests.Services;

public class DocumentServiceTests
{
    private readonly Mock<ILogger<DocumentService>> _mockLogger;
    private readonly DocumentService _documentService;

    public DocumentServiceTests()
    {
        _mockLogger = new Mock<ILogger<DocumentService>>();
        _documentService = DocumentServiceBuilder.Build(_mockLogger.Object);
    }

    [Fact]
    public async Task IngestAsync_WithValidDocumentName_ShouldReturnTrue()
    {
        // Arrange
        var documentName = "test-document.pdf";

        // Act
        var result = await _documentService.IngestAsync(documentName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IngestAsync_WithNullDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.IngestAsync(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IngestAsync_WithEmptyDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.IngestAsync("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IngestAsync_WithWhitespaceDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.IngestAsync("   ");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task IngestAsync_WithDuplicateDocumentName_ShouldReturnFalse()
    {
        // Arrange
        var documentName = "duplicate-document.pdf";
        await _documentService.IngestAsync(documentName);

        // Act
        var result = await _documentService.IngestAsync(documentName);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WithExistingDocument_ShouldReturnTrue()
    {
        // Arrange
        var documentName = "existing-document.pdf";
        await _documentService.IngestAsync(documentName);

        // Act
        var result = await _documentService.DeleteAsync(documentName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingDocument_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.DeleteAsync("non-existing-document.pdf");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WithNullDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.DeleteAsync(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_WithEmptyDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.DeleteAsync("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ListAsync_WithNoDocuments_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _documentService.ListAsync();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task ListAsync_WithMultipleDocuments_ShouldReturnAllDocuments()
    {
        // Arrange
        var document1 = "document1.pdf";
        var document2 = "document2.pdf";
        var document3 = "document3.pdf";
        
        await _documentService.IngestAsync(document1);
        await _documentService.IngestAsync(document2);
        await _documentService.IngestAsync(document3);

        // Act
        var result = await _documentService.ListAsync();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(document1);
        result.Should().Contain(document2);
        result.Should().Contain(document3);
    }

    [Fact]
    public async Task ListAsync_AfterDeletingDocument_ShouldNotContainDeletedDocument()
    {
        // Arrange
        var document1 = "document1.pdf";
        var document2 = "document2.pdf";
        
        await _documentService.IngestAsync(document1);
        await _documentService.IngestAsync(document2);
        await _documentService.DeleteAsync(document1);

        // Act
        var result = await _documentService.ListAsync();

        // Assert
        result.Should().HaveCount(1);
        result.Should().NotContain(document1);
        result.Should().Contain(document2);
    }

    [Fact]
    public async Task IngestAsync_ShouldLogInformation()
    {
        // Arrange
        var documentName = "test-document.pdf";

        // Act
        await _documentService.IngestAsync(documentName);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Ingesting document '{documentName}'")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldLogInformation()
    {
        // Arrange
        var documentName = "test-document.pdf";
        await _documentService.IngestAsync(documentName);

        // Act
        await _documentService.DeleteAsync(documentName);

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Deleting document '{documentName}'")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
