using SK.Rag.Application.Models;

namespace SK.Rag.Application.DocumentLoaders.Interfaces;

public interface IDocumentLoader
{
    IAsyncEnumerable<DocumentChunk> StreamChunks(Stream stream, string documentUri);
}
