using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentIngestionHandler(
    IDocumentService documentService,
    ILogger<DocumentIngestionHandler> logger)
{
    private readonly IDocumentService _documentService = documentService ?? throw new ArgumentNullException(nameof(documentService));
    private readonly ILogger<DocumentIngestionHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

}
