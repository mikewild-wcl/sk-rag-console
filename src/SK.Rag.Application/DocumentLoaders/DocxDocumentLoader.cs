using DocumentFormat.OpenXml.Packaging;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;
using System.Text;
using System.Xml;

namespace SK.Rag.Application.DocumentLoaders;

public class DocxDocumentLoader : IDocumentLoader
{
    public async IAsyncEnumerable<DocumentChunk> StreamChunks(Stream stream, string documentUri)
    {
        var paragraphs = ReadParagraphs(stream, documentUri);
        await foreach (var paragraph in paragraphs)
        {
            yield return paragraph;
        }
    }

    // Code adapted from https://github.com/MicrosoftDocs/semantic-kernel-docs/blob/main/semantic-kernel/concepts/vector-store-connectors/how-to/vector-store-data-ingestion.md
    public async static IAsyncEnumerable<DocumentChunk> ReadParagraphs(Stream documentContents, string documentUri)
    {
        using var wordDoc = WordprocessingDocument.Open(documentContents, false);
        if (wordDoc.MainDocumentPart == null)
        {
            yield break;
        }

        // Create an XmlDocument to hold the document contents and load the document contents into the XmlDocument.
        var xmlDoc = new XmlDocument();
        var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
#pragma warning disable S1075 // URIs should not be hardcoded
        nsManager.AddNamespace("w", "http://schemas.openxmlformats.org/wordprocessingml/2006/main");
        nsManager.AddNamespace("w14", "http://schemas.microsoft.com/office/word/2010/wordml");
#pragma warning restore S1075 // URIs should not be hardcoded

        xmlDoc.Load(wordDoc.MainDocumentPart.GetStream());

        // Select all paragraphs in the document and break if none found.
        XmlNodeList? paragraphs = xmlDoc.SelectNodes("//w:p", nsManager);
        if (paragraphs == null)
        {
            yield break;
        }

        foreach (XmlNode paragraph in paragraphs)
        {
            // Select all text nodes in the paragraph and continue if none found.
            XmlNodeList? texts = paragraph.SelectNodes(".//w:t", nsManager);
            if (texts == null)
            {
                continue;
            }

            // Combine all non-empty text nodes into a single string.
            var textBuilder = new StringBuilder();
            foreach (XmlNode text in texts)
            {
                if (!string.IsNullOrWhiteSpace(text.InnerText))
                {
                    textBuilder.Append(text.InnerText);
                }
            }

            // Yield a new TextParagraph if the combined text is not empty.
            var combinedText = textBuilder.ToString();
            if (!string.IsNullOrWhiteSpace(combinedText))
            {
                yield return new DocumentChunk
                {
                    Key = Guid.NewGuid().ToString(),
                    DocumentUri = documentUri,
                    ParagraphId = paragraph.Attributes?["w14:paraId"]?.Value ?? string.Empty,
                    Text = combinedText
                };
            }
        }
    }
}
