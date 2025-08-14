using Microsoft.Extensions.DependencyInjection;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using System;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class CommandBuilder(
    string name,
    string description,
    IServiceProvider serviceProvider,
    Command? baseCommand = null)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public Command Command { get; } = baseCommand ?? new Command(name, description);

    public CommandBuilder AddCommand(Command chatCommand)
    {
        Command.Add(chatCommand);
        return this;
    }

    public CommandBuilder AddChatCommand()
    {
        Command chatCommand = new("chat")
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        chatCommand.Validators.Add(Validators.DocumentOptionsValidator);
        chatCommand.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
        {
            using var serviceScope = _serviceProvider.CreateAsyncScope();
            var action = _serviceProvider.GetRequiredService<ChatAction>();
            await action.Run(parseResult, cancellationToken);
        });

        Command.Add(chatCommand);

        return this;
    }

    public CommandBuilder AddDocumentCommands()
    {
        Command documentIngestCommand = new("ingest", "Ingest a document")
        {
            Options.DirectoryOption,
            Options.FileOption,
            Options.UriOption
        };

        documentIngestCommand.Validators.Add(Validators.DocumentOptionsValidator);
        documentIngestCommand.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
        {
            using var serviceScope = _serviceProvider.CreateAsyncScope();
            var action = _serviceProvider.GetRequiredService<DocumentIngestAction>();
            await action.Run(parseResult, cancellationToken);
        });
        
        Command documentCommand = new("document", "Manage documents")
        {
            Aliases = { "doc" },
            Subcommands =
            {
                documentIngestCommand
            },
        };

        Command.Add(documentCommand);

        return this;
    }
}
