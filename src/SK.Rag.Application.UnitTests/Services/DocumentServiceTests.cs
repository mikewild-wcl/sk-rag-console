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
        _documentService = DocumentServiceBuilder.Build(logger: _mockLogger.Object);
    }

    [Fact]
    public async Task Ingest_WithValidDocumentName_ShouldReturnTrue()
    {
        // Arrange
        var documentName = "test-document.pdf";

        // Act
        var result = await _documentService.Ingest(documentName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Ingest_WithNullDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Ingest(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Ingest_WithEmptyDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Ingest("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Ingest_WithWhitespaceDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Ingest("   ");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Ingest_WithDuplicateDocumentName_ShouldReturnFalse()
    {
        // Arrange
        var documentName = "duplicate-document.pdf";
        await _documentService.Ingest(documentName);

        // Act
        var result = await _documentService.Ingest(documentName);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_WithExistingDocument_ShouldReturnTrue()
    {
        // Arrange
        var documentName = "existing-document.pdf";
        await _documentService.Ingest(documentName);

        // Act
        var result = await _documentService.Delete(documentName);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task Delete_WithNonExistingDocument_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Delete("non-existing-document.pdf");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_WithNullDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Delete(null!);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task Delete_WithEmptyDocumentName_ShouldReturnFalse()
    {
        // Act
        var result = await _documentService.Delete("");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task List_WithNoDocuments_ShouldReturnEmptyCollection()
    {
        // Act
        var result = await _documentService.List();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task List_WithMultipleDocuments_ShouldReturnAllDocuments()
    {
        // Arrange
        var document1 = "document1.pdf";
        var document2 = "document2.pdf";
        var document3 = "document3.pdf";

        await _documentService.Ingest(document1);
        await _documentService.Ingest(document2);
        await _documentService.Ingest(document3);

        // Act
        var result = await _documentService.List();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(document1);
        result.Should().Contain(document2);
        result.Should().Contain(document3);
    }

    [Fact]
    public async Task List_AfterDeletingDocument_ShouldNotContainDeletedDocument()
    {
        // Arrange
        var document1 = "document1.pdf";
        var document2 = "document2.pdf";

        await _documentService.Ingest(document1);
        await _documentService.Ingest(document2);
        await _documentService.Delete(document1);

        // Act
        var result = await _documentService.List();

        // Assert
        result.Should().HaveCount(1);
        result.Should().NotContain(document1);
        result.Should().Contain(document2);
    }

    [Fact]
    public async Task Ingest_ShouldLogInformation()
    {
        // Arrange
        var documentName = "test-document.pdf";

        // Act
        await _documentService.Ingest(documentName);

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
    public async Task Delete_ShouldLogInformation()
    {
        // Arrange
        var documentName = "test-document.pdf";
        await _documentService.Ingest(documentName);

        // Act
        await _documentService.Delete(documentName);

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
