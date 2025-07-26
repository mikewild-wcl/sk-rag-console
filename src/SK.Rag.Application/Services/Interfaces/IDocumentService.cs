namespace SK.Rag.Application.Services.Interfaces;

public interface IDocumentService
{
    Task<bool> Ingest(string documentName);

    Task<bool> Delete(string documentName);

    Task<IEnumerable<string>> List();
}
