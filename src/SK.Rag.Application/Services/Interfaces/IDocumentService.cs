namespace SK.Rag.Application.DocumentLoaders.Interfaces;

public interface IDocumentService
{
    Task<bool> IngestAsync(string documentName);
    Task<bool> DeleteAsync(string documentName);
    Task<IEnumerable<string>> ListAsync();
}
