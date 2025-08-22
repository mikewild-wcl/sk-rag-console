using SK.Rag.Application.Models;

namespace SK.Rag.Application.Services.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<DocumentChunk>> SemanticSearch(string? queryText, int maxResults = 3, CancellationToken cancellationToken = default);
}