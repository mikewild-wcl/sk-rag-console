using SK.Rag.CommandLine.ConsoleApp.Extensions;
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
        Command command = new("chat")
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        command.Validators.Add(Validators.DocumentOptionsValidator);
        command.SetActionWithServiceScope<ChatAction>(_serviceProvider);

        Command.Add(command);

        return this;
    }

    public CommandBuilder AddDocumentCommands()
    {
        var documentIngestCommand = CreateDocumentIngestCommand();
        var documentListCommand = CreateDocumentListCommand();

        Command documentCommand = new("document", "Manage documents")
        {
            Aliases = { "doc", "documents" },
            Subcommands =
            {
                documentIngestCommand,
                documentListCommand
            },
        };

        Command.Add(documentCommand);

        return this;
    }

    public Command CreateDocumentIngestCommand()
    {
        Command command = new("ingest", "Ingest a document")
        {
            Options.DirectoryOption,
            Options.FileOption,
            Options.UriOption
        };

        command.Validators.Add(Validators.DocumentOptionsValidator);
        command.SetActionWithServiceScope<DocumentIngestAction>(_serviceProvider);

        return command;
    }

    public Command CreateDocumentDeleteCommand()
    {
        Command command = new("delete", "List documents")
        {
            Aliases = { "del" },
        };
        command.SetActionWithServiceScope<DocumentDeleteAction>(_serviceProvider);

        return command;
    }

    public Command CreateDocumentListCommand()
    {
        Command command = new("list", "List documents")
        {
            Aliases = { "ls" },
        };

        command.SetActionWithServiceScope<DocumentListAction>(_serviceProvider);

        return command;
    }
}
