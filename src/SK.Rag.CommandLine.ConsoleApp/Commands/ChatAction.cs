using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class ChatAction(
    IAnsiConsole console,
    IChatService chatService,
    IDocumentService _documentService,
    IServiceProvider serviceProvider,
    ILogger<ChatAction> logger)
{
    private readonly IChatService _chatService = chatService;
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<ChatAction> _logger = logger;

    public async Task Run(
        IEnumerable<FileInfo> files,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting new chat session");

        //_console.Clear();
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
    }

    private bool TryParseInputAsCommand(string? userInput, CancellationToken cancellationToken)
    {
        if (userInput?.TrimStart() is not { Length: > 1 } ||
           !userInput.TrimStart().StartsWith('/'))
        {
            return false;
        }

        //var commandString = userInput.Substring(userInput.IndexOf('/') + 1).TrimEnd();
        var commandString = userInput.Trim().Substring(1);

        //Write a regex that finds a string with a leading slash and returns the string value without leading or trailing whitespace
        var match = Regex.Match(userInput ?? "", @"^\s*/(.+?)\s*$");
        var commandValue = match.Success ? match.Groups[1].Value : string.Empty;

        //Write a regex that finds a string with a leading slash and returns the string value without leading or trailing whitespace

        //var command = _serviceProvider.GetRequiredKeyedService<Command>("SlashCommand");
        var commandBuilder = new CommandBuilder("Slash", "Slash command", _serviceProvider);
        commandBuilder.AddDocumentCommands();
        var command = commandBuilder.Command;

        var parseResult = command.Parse(commandString);

        if (parseResult.Errors.Count > 0)
        {
            foreach (var error in parseResult.Errors)
            {
                _logger.LogError("Command parsing error: {ErrorMessage}", error.Message);
                AnsiConsole.MarkupLine($"[red]Error:[/] {error.Message}");
            }
            return false;
        }

        if (parseResult.Tokens.Count > 0)
        {
            var commandResult = parseResult.Invoke();
            //var commandResult = await parseResult.InvokeAsync(cancellationToken);

            return commandResult! >= 0;
        }

        return false;
    }
}
