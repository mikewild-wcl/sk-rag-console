using Microsoft.Extensions.VectorData;

namespace SK.Rag.Application.Models;

public class DocumentChunk
{
    [VectorStoreKey]
    public required string Key { get; init; }

    [VectorStoreData]
    public required string DocumentUri { get; init; }

    [VectorStoreData]
    public required string ParagraphId { get; init; }

    [VectorStoreData(IsFullTextIndexed = true)]
    public required string Text { get; init; }

    [VectorStoreVector(1536)]
    public ReadOnlyMemory<float> TextEmbedding { get; set; }
}
