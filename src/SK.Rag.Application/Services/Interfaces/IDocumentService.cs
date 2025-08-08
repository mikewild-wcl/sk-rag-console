namespace SK.Rag.Application.Services.Interfaces;

public interface IDocumentService
{
    Task Ingest(IEnumerable<FileInfo> files, CancellationToken cancellationToken);

    Task<bool> Delete(string documentName);

    Task<IEnumerable<string>> List();
}
