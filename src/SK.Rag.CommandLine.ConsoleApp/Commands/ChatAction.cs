using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;
using System.Text.RegularExpressions;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public partial class ChatAction(
    IAnsiConsole console,
    IChatService chatService,
    IDocumentService _documentService,
    IServiceProvider serviceProvider,
    ILogger<ChatAction> logger) : ICommandActionRunner
{
    private readonly IChatService _chatService = chatService;
    private readonly IDocumentService _documentService = _documentService;
    private readonly IAnsiConsole _console = console;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<ChatAction> _logger = logger;

    // Finds a string with a leading slash and returns the string value without leading or trailing whitespace
    [GeneratedRegex(@"^\s*/(.+?)\s*$")]
    private static partial Regex SlashCommandRegex();

    public async Task Run(ParseResult parseResult, CancellationToken cancellationToken)
    {
        var files = parseResult.GetFileList();

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

            if (await TryParseInputAsCommand(userInput, cancellationToken))
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
      
    private async Task<bool> TryParseInputAsCommand(string? userInput, CancellationToken cancellationToken)
    {
        var match = SlashCommandRegex().Match(userInput ?? "");
        var commandString = match.Success ? match.Groups[1].Value : string.Empty;

        if(!match.Success || string.IsNullOrWhiteSpace(commandString))
        {
            return false;
        }

        var commandBuilder = new CommandBuilder("Slash", "Slash command", _serviceProvider);
        commandBuilder.AddDocumentCommands();
        var command = commandBuilder.Command;

        var parseResult = command.Parse(commandString);

        if (parseResult.Errors.Count > 0)
        {
            foreach (var error in parseResult.Errors.Select(e => e.Message))
            {
                _logger.LogError("Command parsing error: {ErrorMessage}", error);
                AnsiConsole.MarkupLine($"[red]Error:[/] {error}");
            }

            return false;
        }

        if (parseResult.Tokens.Count > 0)
        {
            var commandResult = await parseResult.InvokeAsync(cancellationToken: cancellationToken);
            return commandResult! >= 0;
        }

        return false;
    }
}
