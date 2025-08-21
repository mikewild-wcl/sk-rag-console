using Microsoft.SemanticKernel.Text;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.PageSegmenter;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;

namespace SK.Rag.Application.DocumentLoaders;

public class PdfDocumentLoader : IDocumentLoader
{
    public async IAsyncEnumerable<DocumentChunk> StreamChunks(Stream stream, string documentUri)
    {
        //if (string.IsNullOrEmpty(documentUri))
        //{
        //    yield break;
        //}

        //if(!File.Exists(documentUri))
        //{
        //    yield break;
        //}

        using var pdf = PdfDocument.Open(stream);
        var pages = pdf.GetPages();
        var paragraphs = pages.SelectMany(GetPageParagraphs);

        foreach (var paragraph in paragraphs)
        {
            yield return new DocumentChunk
            {
                Key = Guid.NewGuid().ToString(),
                DocumentUri = documentUri,
                //PageNumber = paragraph.PageNumber,
                //Index = paragraph.IndexOnPage,
                ParagraphId = string.Empty,
                Text = paragraph.Text
            };
        }
    }

    private static IEnumerable<(int PageNumber, int IndexOnPage, string Text)> GetPageParagraphs(Page pdfPage)
    {
        var letters = pdfPage.Letters;
        var words = NearestNeighbourWordExtractor.Instance.GetWords(letters);

        //TODO: use a filter on PdfDocument.Open to do this
        var sep = Convert.ToChar(61623);
        var cleanedWords = words
            .Where(word => !(word.Letters?.Count > 0 &&
                             word.Letters[0]?.Value?.Length == 1 &&
                             word.Letters[0].Value[0] == sep))
            .ToList();

        //foreach (var word in words)
        //{
        //    if (word.Letters?.Count == 1 && word.Letters[0]?.Value?.Length == 1 && word.Letters[0].Value[0] == Convert.ToChar(61623))
        //    {
        //        // Skip single 'x' letters, which are often used as checkboxes or similar markers.
        //    }
        //}

        var textBlocks = DocstrumBoundingBoxes.Instance.GetBlocks(cleanedWords);
        var pageText = string.Join(Environment.NewLine + Environment.NewLine,
            textBlocks.Select(t => t.Text.ReplaceLineEndings(" ")));

#pragma warning disable SKEXP0050 // Type is for evaluation purposes only
        return TextChunker.SplitPlainTextParagraphs([pageText], Constants.MaxTokensPerParagraph)
            .Select((text, index) => (pdfPage.Number, index, text));
#pragma warning restore SKEXP0050 // Type is for evaluation purposes only
    }
}
