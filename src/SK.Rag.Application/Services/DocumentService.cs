using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.VectorData;
using Microsoft.SemanticKernel;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Models;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.Services;

public class DocumentService(
    IDocumentLoaderFactory documentLoaderFactory,
    Kernel kernel,
    ILogger<DocumentService> logger) : IDocumentService
{
    private readonly IDocumentLoaderFactory _documentLoaderFactory = documentLoaderFactory;
    private readonly ILogger<DocumentService> _logger = logger;
    private readonly Kernel _kernel = kernel;

    private static readonly List<string> _documents = []; //Static so it can be used across multiple instances of the service. No logging implemented!

    public async Task Ingest(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting document ingestion. Files count: {Count}", files.Count());

            //TODO: Consider breaking out into private methods for better readability

            //    if (!files.Any())
            //    {
            //        _logger.LogWarning("No files provided for ingestion.");
            //        return;
            //    }
            //    await IngestDocumentsAsync(files, cancellationToken);

            var vectorCollection = await _kernel.GetVectorStoreCollection<string, DocumentChunk>(Constants.DocumentCollectionName);

            foreach (var file in files)
            {
                var documentType = file.GetDocumentType();
                var documentLoader = _documentLoaderFactory.Create(documentType);

                using var stream = file.OpenRead();

                var embeddingGenerator = _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();

                await foreach (var chunk in documentLoader.StreamChunks(stream, file.FullName))
                {
                    //TODO: Pass this async stream into a vector store
                    //yield return item;
                    _logger.LogInformation("Processing chunk from document '{DocumentName}'. {Key} - {Text}",
                        file.Name,
                        chunk.Key,
                        chunk.Text);

                    chunk.TextEmbedding = await embeddingGenerator.GenerateVectorAsync(chunk.Text);
                    await vectorCollection.UpsertAsync(chunk);
                }

                _documents.Add(file.Name);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during document ingestion.");

            //TODO: Consider returning a Result type or similar to indicate success/failure
        }
    }

    [Obsolete("This method is deprecated. Use Ingest(IEnumerable<FileInfo> files, CancellationToken cancellationToken) instead.")]
    public async Task<bool> Ingest(string documentName)
    {
        _logger.LogInformation("Ingesting document '{DocumentName}'", documentName);

        if (string.IsNullOrWhiteSpace(documentName))
        {
            _logger.LogWarning("Document name cannot be null or empty");
            return false;
        }

        if (_documents.Contains(documentName))
        {
            _logger.LogWarning("Document '{DocumentName}' already exists", documentName);
            return false;
        }

        _documents.Add(documentName);
        _logger.LogInformation("Successfully ingested document '{DocumentName}'", documentName);

        return await Task.FromResult(true);
    }

    public async Task<bool> Delete(string documentName)
    {
        _logger.LogInformation("Deleting document '{DocumentName}'", documentName);

        if (string.IsNullOrWhiteSpace(documentName))
        {
            _logger.LogWarning("Document name cannot be null or empty");
            return false;
        }

        var removed = _documents.Remove(documentName);
        if (removed)
        {
            _logger.LogInformation("Successfully deleted document '{DocumentName}'", documentName);
        }
        else
        {
            _logger.LogWarning("Document '{DocumentName}' not found", documentName);
        }

        return await Task.FromResult(removed);
    }

    public async Task<IEnumerable<string>> List()
    {
        _logger.LogInformation("Listing all documents. Count: {Count}", _documents.Count);
        return await Task.FromResult(_documents.AsReadOnly());
    }

    //private async Task<VectorStoreCollection<string, DocumentChunk>> GetVectorStoreCollection()
    //{
    //    var vectorStore = _kernel.GetRequiredService<VectorStore>();

    //    var collection = vectorStore.GetCollection<string, DocumentChunk>(Constants.DocumentCollectionName);
    //    await collection.EnsureCollectionExistsAsync();

    //    return collection;
    //}

    //TODO: Move to SemanticKernelExtensions extensions class

    private async Task IngestDocumentsAsync(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        var vectorStore = _kernel.GetRequiredService<VectorStore>();
        var collection = vectorStore.GetCollection<string, DocumentChunk>(Constants.DocumentCollectionName);
        await collection.EnsureCollectionExistsAsync();
        foreach (var file in files)
        {
            var documentType = file.GetDocumentType();
            var documentLoader = _documentLoaderFactory.Create(documentType);
            using var stream = file.OpenRead();
            var embeddingGenerator = _kernel.GetRequiredService<IEmbeddingGenerator<string, Embedding<float>>>();
            await foreach (var chunk in documentLoader.StreamChunks(stream, file.FullName))
            {
                chunk.TextEmbedding = await embeddingGenerator.GenerateVectorAsync(chunk.Text);
                await collection.UpsertAsync(chunk);
            }
            _documents.Add(file.Name);
        }
    }
}
