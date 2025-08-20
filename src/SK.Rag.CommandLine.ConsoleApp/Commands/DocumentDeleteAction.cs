using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentDeleteAction(
    IAnsiConsole console,
    IDocumentService _documentService,
    ILogger<DocumentDeleteAction> logger) : ICommandActionRunner
{
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly ILogger<DocumentDeleteAction> _logger = logger;

    public async Task Run(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var files = parseResult.GetFileList();
        if(files is null || !files.Any())
        {
            _console.MarkupLine("[amber]No documents specified for deletion.[/]");
            return;
        }

        foreach (var file in files)
        {
            _console.MarkupLine($"Deleting document: {file.Name}");
            var isDeleted = await _documentService.Delete(file.Name);
            
            if(!isDeleted)
            {
                _console.MarkupLine($"[red]Failed to delete document: {file.Name}[/]");
            }
        }
    }
}
