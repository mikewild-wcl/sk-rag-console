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

        var prompt = "test";

        // await chatService.Chat(prompt, cancellationToken);

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

            if(userInput?.Trim() == "/q" || userInput?.Trim() == "/quit")
            {
                _logger.LogInformation("User is quitting the session with {UserInput}", userInput);
                break;
            }

            await foreach (var responseToken in chatService.GetResponseAsync(userInput, cancellationToken))
            {
                console.Write(responseToken ?? "");
            }

            console.WriteLine();

        } while (true);

        return context;
    }
}
