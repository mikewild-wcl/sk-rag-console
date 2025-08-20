using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentIngestAction(
    IAnsiConsole console,
    IDocumentService _documentService,
    ILogger<DocumentIngestAction> logger) : ICommandActionRunner
{
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly ILogger<DocumentIngestAction> _logger = logger;

    public async Task Run(ParseResult parseResult, CancellationToken cancellationToken)
    {
        _console.WriteLine("Ingesting document(s)");
        var uri = parseResult.GetValue(Options.UriOption);
        if (uri is not null)
        {
            _console.WriteLine($"Uri - {uri.AbsolutePath} IsFile={uri.IsFile}, IsLoopback={uri.IsLoopback}, LocalPath={uri.LocalPath}");
        }

        var files = parseResult.GetFileList();
        await _documentService.Ingest(files, cancellationToken);

        //TODO: Create a handler from services and pass the dir/file list(s) to it
        //      Create a service scope as well
        //      Also pass dir/files ti chat command and start on doc loading
        //      Add parser to services (or a simpler parser)  
    }    
}
