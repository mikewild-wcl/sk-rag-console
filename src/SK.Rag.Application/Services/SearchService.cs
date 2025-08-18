using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Models;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.Services;

public class SearchService(
    Kernel kernel,
    ILogger<SearchService> logger) : ISearchService
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<SearchService> _logger = logger;

    public async Task<IEnumerable<DocumentChunk>> SemanticSearch(string? text, int maxResults = 3)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            _logger.LogWarning("Search text is null or empty");
            return Enumerable.Empty<DocumentChunk>();
        }

        try
        {
            var vectorCollection = await _kernel.GetVectorStoreCollection<string, DocumentChunk>(Constants.DocumentCollectionName);
            var nearest = vectorCollection.SearchAsync(text, maxResults);
            return await nearest.Select(result => result.Record).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during semantic search");
            return Enumerable.Empty<DocumentChunk>();
        }
    }
}