using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;
using System.Text.RegularExpressions;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentIngestAction(
    IAnsiConsole console,
    IDocumentService _documentService,
    IServiceProvider serviceProvider,
    ILogger<DocumentIngestAction> logger)
{
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<DocumentIngestAction> _logger = logger;

    public async Task Run(ParseResult parseResult, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Ingesting document(s)");
        var uri = parseResult.GetValue(Options.UriOption);
        if (uri is not null)
        {
            Console.WriteLine($"Uri - {uri.AbsolutePath} IsFile={uri.IsFile}, IsLoopback={uri.IsLoopback}, LocalPath={uri.LocalPath}");
        }

        var files = parseResult.GetFileList();
        await _documentService.Ingest(files, cancellationToken);

        //TODO: Create a handler from services and pass the dir/file list(s) to it
        //      Create a service scope as well
        //      Also pass dir/files ti chat command and start on doc loading
        //      Add parser to services (or a simpler parser)  
    }

    public async Task Run(
        IEnumerable<FileInfo> files,
        CancellationToken cancellationToken)
    {
        /*
        _logger.LogInformation("Starting new chat session");

        _console.WriteApplicationFigletText();

        if (files.Any())
        {
            await _documentService.Ingest(files, cancellationToken);
        }

        string? userInput;
        do
        {
            _console.Write("User > ");
            userInput = Console.ReadLine();

            if (userInput is not { Length: > 0 })
            {
                continue;
            }

            _console.Write("Response > ");

            if (userInput?.Trim() == "/q" || userInput?.Trim() == "/quit")
            {
                _logger.LogInformation("User is quitting the session with {UserInput}", userInput);
                break;
            }

            if (TryParseInputAsCommand(userInput, cancellationToken))
            {
                continue;
            }

            await foreach (var responseToken in _chatService.GetResponseAsync(userInput, cancellationToken))
            {
                _console.Write(responseToken ?? "");
            }

            _console.WriteLine();

        } while (true);
        */
    }
   
}
