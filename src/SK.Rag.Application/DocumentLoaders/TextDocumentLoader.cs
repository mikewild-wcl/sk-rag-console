using Microsoft.SemanticKernel.Text;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;

namespace SK.Rag.Application.DocumentLoaders;

public class TextDocumentLoader : IDocumentLoader
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

        //TODO Implement text reader and chunk by size

        //var pages = pdf.GetPages();
        //var paragraphs = pages.SelectMany(GetPageParagraphs);

        //foreach (var paragraph in paragraphs)
        //{
        //    yield return $"{paragraph.PageNumber} - {paragraph.IndexOnPage} {paragraph.Text}";
        //}
        yield break;
    }

//    private static IEnumerable<(int PageNumber, int IndexOnPage, string Text)> GetPageParagraphs(Page pdfPage)
//    {
//        var letters = pdfPage.Letters;
//        var words = NearestNeighbourWordExtractor.Instance.GetWords(letters);
//        var textBlocks = DocstrumBoundingBoxes.Instance.GetBlocks(words);
//        var pageText = string.Join(Environment.NewLine + Environment.NewLine,
//            textBlocks.Select(t => t.Text.ReplaceLineEndings(" ")));

//#pragma warning disable SKEXP0050 // Type is for evaluation purposes only
//        return TextChunker.SplitPlainTextParagraphs([pageText], Constants.MaxTokensPerParagraph)
//            .Select((text, index) => (pdfPage.Number, index, text));
//#pragma warning restore SKEXP0050 // Type is for evaluation purposes only
//    }
}