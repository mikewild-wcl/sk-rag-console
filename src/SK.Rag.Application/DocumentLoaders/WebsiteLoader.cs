using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.DocumentLoaders;

public class WebsiteLoader(
    IHtmlWebProvider htmlWebProvider,
    ILogger<WebsiteLoader> logger) : IDocumentLoader
{
    public readonly IHtmlWebProvider _htmlWeb = htmlWebProvider;
    public readonly ILogger<WebsiteLoader> _logger = logger;

    public async IAsyncEnumerable<DocumentChunk> StreamChunks(Stream stream, string documentUri)
    {
        var textNodes = await GetTextNodes(stream, documentUri);

        if (textNodes is null)
        {
            yield break;
        }

        foreach (var item in textNodes
            .Where(n => n.ParentNode?.Name != "script" &&
                         n.ParentNode?.Name != "style" &&
                         !string.IsNullOrWhiteSpace(n.InnerText)))
        {
            yield return new DocumentChunk
            {
                Key = Guid.NewGuid().ToString(),
                DocumentUri = documentUri,
                //PageNumber = paragraph.PageNumber,
                //Index = paragraph.IndexOnPage,
                ParagraphId = string.Empty,
                //Position = item.StreamPosition,
                Text = item.InnerText
            };
        }
    }

    private async Task<HtmlNodeCollection?> GetTextNodes(Stream stream, string uri)
    {
        try
        {
            // https://stackoverflow.com/questions/4182594/grab-all-text-from-html-with-html-agility-pack

            var htmlDocument = await _htmlWeb.LoadFromWebAsync(uri);

            var textNodes = htmlDocument
                .DocumentNode
                .SelectNodes("//text()");

            //IEnumerable < HtmlNode> nodes = doc.DocumentNode.Descendants().Where(n =>
            //    n.NodeType == HtmlNodeType.Text &&
            //    n.ParentNode.Name != "script" &&
            //    n.ParentNode.Name != "style");

            return textNodes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to access download page at {Uri}", uri);
        }

        return default;
    }
}
