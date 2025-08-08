using SK.Rag.Application.Models;

namespace SK.Rag.Application.Extensions;

public static class FileInfoExtensions
{
    public static DocumentType GetDocumentType(this FileInfo fileInfo)
    {
        return fileInfo.Extension?.ToLowerInvariant() switch
        {
            ".docx" => DocumentType.Docx,
            ".pdf" => DocumentType.Pdf,
            ".md" => DocumentType.Markdown,
            ".txt" => DocumentType.Text,
            //_ => fileInfo.StartsWith("http://") || fileInfo.StartsWith("https://")
            //    ? DocumentType.WebPage
            //    : DocumentType.Unknown
            _ => DocumentType.Unknown
        };
    }
}
