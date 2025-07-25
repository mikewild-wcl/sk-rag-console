using SK.Rag.Application.DocumentLoaders.Interfaces;
using SK.Rag.Application.Models;

namespace SK.Rag.Application.DocumentLoaders.Interfaces;

public interface IDocumentLoaderFactory
{
    IDocumentLoader Create(DocumentType documentType);
}
