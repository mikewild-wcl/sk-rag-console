using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentListAction(
    IAnsiConsole console,
    IDocumentService _documentService,
    IServiceProvider serviceProvider,
    ILogger<DocumentListAction> logger) : ICommandActionRunner
{
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly ILogger<DocumentListAction> _logger = logger;

    public async Task Run(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var files = await _documentService.List();

        foreach (var file in files)
        {
            _console.MarkupLineInterpolated($"\t[green]{file}[/]");
        }
    }
}
