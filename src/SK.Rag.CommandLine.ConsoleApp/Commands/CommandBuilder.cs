using Microsoft.Extensions.DependencyInjection;
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
        Command chatCommand = new("chat")
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        chatCommand.Validators.Add(Validators.DocumentOptionsValidator);
        chatCommand.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
        {
            using var serviceScope = _serviceProvider.CreateAsyncScope();

            var chatAction = _serviceProvider.GetRequiredService<ChatAction>();

            var files = parseResult.GetFileList();
            await chatAction.Run(
                files,
                cancellationToken);
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
        /*
        documentIngestCommand.SetAction((ParseResult parseResult) =>
        {
            Console.WriteLine($"Ingesting document(s)");
            var dir = parseResult.GetValue(Options.DirectoryOption);
            var file = parseResult.GetValue(Options.FileOption);
            var files = parseResult.GetValue(Options.FilesOption);
            var uri = parseResult.GetValue(Options.UriOption);

            if (dir is not null)
            {
                Console.WriteLine($"Directory - {dir.FullName} Exists={dir.Exists}");
            }

            if (file is not null)
            {
                Console.WriteLine($"File - {file.FullName} Exists={file.Exists}");
            }

            if (uri is not null)
            {
                Console.WriteLine($"Uri - {uri.AbsolutePath} IsFile={uri.IsFile}, IsLoopback={uri.IsLoopback}, LocalPath={uri.LocalPath}");
            }

            if (files is { Length: > 0 })
            {
                foreach (var f in files)
                {
                    Console.WriteLine($"File - {f.FullName} Exists={f.Exists}");
                }
            }

            //TODO: Create a handler from services and pass the dir/file list(s) to it
            //      Create a service scope as well
            //      Also pass dir/files ti chat command and start on doc loading
            //      Add parser to services (or a simpler parser)  
        });
        */

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
