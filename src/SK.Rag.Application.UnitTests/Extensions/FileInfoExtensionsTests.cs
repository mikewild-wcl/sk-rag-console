using SK.Rag.Application.Extensions;
using SK.Rag.Application.Models;

namespace SK.Rag.Application.UnitTests.Extensions;

public class FileInfoExtensionsTests
{
    [Theory]
    [InlineData("test.docx", DocumentType.Docx)]
    [InlineData("test.pdf", DocumentType.Pdf)]
    [InlineData("test.md", DocumentType.Markdown)]
    [InlineData("test.txt", DocumentType.Text)]
    [InlineData("test.unknown", DocumentType.Unknown)]
    [InlineData("test", DocumentType.Unknown)]
    public void GetDocumentType_ReturnsExpectedType(string fileName, DocumentType expectedType)
    {
        var fileInfo = new FileInfo(fileName);

        var result = fileInfo.GetDocumentType();

        result.Should().Be(expectedType);
    }
}