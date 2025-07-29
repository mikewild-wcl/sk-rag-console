using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using System.CommandLine;
using System.Text;

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
        var prompt = "test";

        await chatService.Chat(prompt, cancellationToken);

        string? userInput;
        do
        {
            Console.Write("User > ");
            userInput = Console.ReadLine();

            if (userInput is not { Length: >0 })
            {
                continue;
            }

            Console.WriteLine("Response > ");

            await foreach (var responseToken in chatService.GetResponseAsync(userInput, cancellationToken))
            {
                Console.Write(responseToken);
            }

            Console.WriteLine();

        } while (userInput?.Trim() != "/q");

        return context;
    }

    public async Task DoSomething(IChatService service, CancellationToken cancellationToken)
    {
        Console.WriteLine("Here we are...");
    }
}
