using Microsoft.Extensions.Logging;
using SK.Rag.Application.Extensions;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.Services;

public class DocumentService(
    IDocumentLoaderFactory documentLoaderFactory,
    ILogger<DocumentService> logger) : IDocumentService
{
    private readonly IDocumentLoaderFactory _documentLoaderFactory = documentLoaderFactory;
    private readonly ILogger<DocumentService> _logger = logger;
    private readonly List<string> _documents = [];

    public async Task Ingest(IEnumerable<FileInfo> files, CancellationToken cancellationToken)
    {
        foreach (var file in files)
        {
            var documentType = file.GetDocumentType();
            var documentLoader = _documentLoaderFactory.Create(documentType);

            using var stream = file.OpenRead();

            await foreach (var item in documentLoader.StreamChunks(stream, file.FullName))
            {
                //TODO: Pass this async stream into a vector store
                //yield return item;
                _logger.LogInformation("Processing chunk from document '{DocumentName}'. {Key} - {Text}", 
                    file.Name,
                    item.Key,
                    item.Text);

            }

            _documents.Add(file.Name);
        }
    }

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
}
