using SK.Rag.Application.Models;

namespace SK.Rag.Application.Services.Interfaces;

public interface ISearchService
{
    Task<IEnumerable<DocumentChunk>> SemanticSearch(string? text, int maxResults = 3);
}