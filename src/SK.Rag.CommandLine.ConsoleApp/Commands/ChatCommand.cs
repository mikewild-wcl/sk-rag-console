using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class ChatCommand : Command
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ChatCommand> _logger;

    public ChatCommand(
        IServiceProvider serviceProvider,
        ILogger<ChatCommand> logger)
        : base("chat", "Starts an interactive chat session.")
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        SetAction(ExecuteAction);
    }

    public async Task<ParseResult> ExecuteAction(ParseResult context, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Executing HelloWithServiceCommand");
        var chatService = _serviceProvider.GetRequiredService<IChatService>();
        var console = _serviceProvider.GetRequiredService<IAnsiConsole>();

        console.Clear();
        console.WriteApplicationFigletText();

        string? userInput;
        do
        {
            console.Write("User > ");
            userInput = Console.ReadLine();

            if (userInput is not { Length: > 0 })
            {
                continue;
            }

            console.Write("Response > ");

            if (userInput?.Trim() == "/q" || userInput?.Trim() == "/quit")
            {
                _logger.LogInformation("User is quitting the session with {UserInput}", userInput);
                break;
            }

            if (TryParseInputAsCommand(userInput, cancellationToken))
            {
                continue;
            }

            await foreach (var responseToken in chatService.GetResponseAsync(userInput, cancellationToken))
            {
                console.Write(responseToken ?? "");
            }

            console.WriteLine();

        } while (true);

        return context;
    }

    private bool TryParseInputAsCommand(string? userInput, CancellationToken cancellationToken)
    {
        if (userInput?.TrimStart() is not { Length: > 1 } ||
           !userInput.TrimStart().StartsWith('/'))
        {
            return false;
        }

        var commandString = userInput[userInput.IndexOf('/')..].TrimEnd();

        var command = _serviceProvider.GetRequiredService<RootCommand>();
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
            var commandRsult = parseResult.Invoke();
            //var commandRsult = await parseResult.InvokeAsync(cancellationToken);

            return commandRsult! >= 0;
        }

        return false;
    }
}
