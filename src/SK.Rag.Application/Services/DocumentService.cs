using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;

namespace SK.Rag.Application.DocumentLoaders;

public class DocumentService(
    ILogger<DocumentService> logger) : IDocumentService
{
    private readonly ILogger<DocumentService> _logger = logger;
    private readonly List<string> _documents = [];

    public async Task<bool> IngestAsync(string documentName)
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

    public async Task<bool> DeleteAsync(string documentName)
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

    public async Task<IEnumerable<string>> ListAsync()
    {
        _logger.LogInformation("Listing all documents. Count: {Count}", _documents.Count);
        return await Task.FromResult(_documents.AsReadOnly());
    }
}
