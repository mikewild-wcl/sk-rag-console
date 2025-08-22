using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.Services;

public class SearchService(
    Kernel kernel,
    ILogger<SearchService> logger) : ISearchService
{
    private readonly Kernel _kernel = kernel;
    private readonly ILogger<SearchService> _logger = logger;

    public async Task<IEnumerable<DocumentChunk>> SemanticSearch(string? queryText, int maxResults = 3, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(queryText))
        {
            _logger.LogWarning("Search text is null or empty");
            return Enumerable.Empty<DocumentChunk>();
        }

        try
        {
            var vectorCollection = await _kernel.GetVectorStoreCollection<string, DocumentChunk>(Constants.DocumentCollectionName);

            var embeddingGenerator = _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
            var queryEmbedding = await embeddingGenerator.GenerateVectorAsync(queryText, cancellationToken: cancellationToken);

            var nearest = vectorCollection.SearchAsync(queryEmbedding, maxResults);
            return await nearest.Select(result => result.Record).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during semantic search");
            return Enumerable.Empty<DocumentChunk>();
        }
    }
}