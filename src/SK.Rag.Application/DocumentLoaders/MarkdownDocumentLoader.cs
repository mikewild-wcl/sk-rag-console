using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;

namespace SK.Rag.Application.DocumentLoaders;

public class MarkdownDocumentLoader : IDocumentLoader
{
    public async IAsyncEnumerable<DocumentChunk> StreamChunks(Stream stream, string documentUri)
    {
        //TODO Implement text reader and chunk by size

        yield break;
    }
}