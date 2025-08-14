using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SK.Rag.CommandLine.ConsoleApp;
using SK.Rag.CommandLine.ConsoleApp.Commands;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using System.CommandLine;

// Look into this - https://endjin.com/blog/2020/09/simple-pattern-for-using-system-commandline-with-dependency-injection
// https://learn.microsoft.com/en-us/dotnet/standard/commandline/migration-guide-2.0.0-beta5

var builder = Host.CreateApplicationBuilder();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddLogging(l => l.AddConsole())
    .AddServices()
    .AddClients()
    .AddSemanticKernel();

//var rootCommand = BuildRootCommand(builder.Services);

//var commandLineConfig = new CommandLineConfiguration(rootCommand);

builder.Build();

/*
Command chatCommand = new("chat", "Start an interactive chat session");
chatCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Hello chatters!");
    // Here you would typically call your chat service
});

Command documentCommand = new("document");
documentCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"This is the basic document command");
});

Command documentIngestCommand = new("ingest", "Ingest a document")
{
    fileOption
};
documentIngestCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Ingesting document");
});

Command documentDeleteCommand = new("delete", "Delete a document")
{
    fileOption
};
documentDeleteCommand.SetAction((parseResult) =>
{
    var file = parseResult.GetValue(fileOption);
    Console.WriteLine($"Deleting document {file}");
});

Command documentListCommand = new("list", "List documents");
documentListCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Listing documents");
    Console.WriteLine($" - document 1");
    Console.WriteLine($" - document 2");
    Console.WriteLine($" - document 3");
});

documentCommand.Subcommands.Add(documentIngestCommand);
documentCommand.Subcommands.Add(documentDeleteCommand);
documentCommand.Subcommands.Add(documentListCommand);

RootCommand rootCommand = new("Sample app for System.CommandLine")
{
    //fileOption
};
rootCommand.Subcommands.Add(chatCommand);
rootCommand.Subcommands.Add(documentCommand);

rootCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Hello rooters!");
    // Here you would typically call your chat service
});

var parseResult = rootCommand.Parse(args);

//if (parseResult.GetValue(fileOption) is FileInfo parsedFile)
//{
//    ReadFile(parsedFile);
//    return 0;
//}

foreach (var parseError in parseResult.Errors)
{
    await Console.Error.WriteLineAsync(parseError.Message);
}

//static void Run(bool boolean, string text)
//{
//    Console.WriteLine($"Bool option: {text}");
//    Console.WriteLine($"String option: {boolean}");
//}

//var builder = new CommandLineBuilder 

//var result = await parseResult.InvokeAsync();
*/

//var config = new CommandLineConfiguration(rootCommand);

var rootCommand = BuildRootCommand(builder.Services);

var commandLineConfig = new CommandLineConfiguration(rootCommand);
var commandResult = await commandLineConfig.InvokeAsync(args);

return commandResult;

//static CommandLineConfiguration BuildParser(ServiceProvider serviceProvider)
static RootCommand BuildRootCommand(IServiceCollection services)
{
    var serviceProvider = services.BuildServiceProvider();

    var commandBuilder = new CommandBuilder(
        "", 
        ApplicationConstants.ApplicationDescription, 
        serviceProvider,
        new RootCommand());

    /*
    Command chatCommand = new("chat")
    {
        Options.DirectoryOption,
        Options.FileOption
    };
    chatCommand.Validators.Add(Validators.DocumentOptionsValidator);
    chatCommand.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
    {
        using var serviceScope = serviceProvider.CreateAsyncScope();

        var chatAction = serviceProvider.GetRequiredService<ChatAction>();

        var dir = parseResult.GetValue(Options.DirectoryOption);
        // TODO: Make this a list of files as well
        var file = parseResult.GetValue(Options.FileOption);

        var files = dir?.EnumerateFiles()?.ToList() ?? [];
        if (file is not null)
        {
            files.Add(file);
        }

        await chatAction.RunChat(
            files,
            cancellationToken);
    });
    */

    commandBuilder
        .AddDocumentCommands()
        .AddChatCommand();

    return (commandBuilder.Command as RootCommand) ?? new RootCommand();

    /* Below here has been moved into builder */

    //Command documentIngestCommand = new("ingest", "Ingest a document")
    //{
    //    Options.DirectoryOption,
    //    Options.FileOption
    //};

    //documentIngestCommand.Validators.Add(Validators.DocumentOptionsValidator);
    //documentIngestCommand.SetAction((ParseResult parseResult) =>
    //{
    //    Console.WriteLine($"Ingesting document(s)");
    //    var dir = parseResult.GetValue(Options.DirectoryOption);
    //    var file = parseResult.GetValue(Options.FileOption);
    //    var files = parseResult.GetValue(Options.FilesOption);

    //    if (dir is not null)
    //    {
    //        Console.WriteLine($"Directory - {dir.FullName} Exists={dir.Exists}");
    //    }

    //    if (file is not null)
    //    {
    //        Console.WriteLine($"File - {file.FullName} Exists={file.Exists}");
    //    }

    //    if (files is { Length: > 0 })
    //    {
    //        foreach (var f in files)
    //        {
    //            Console.WriteLine($"File - {f.FullName} Exists={f.Exists}");
    //        }
    //    }

    //    //TODO: Create a handler from services and pass the dir/file list(s) to it
    //    //      Create a service scope as well
    //    //      Also pass dir/files ti chat command and start on doc loading
    //    //      Add parser to services (or a simpler parser)  

    //});

    //    documentIngestCommand.SetAction((ParseResult parseResult, IServiceProvider sp) =>
    //Command documentCommand = new("document", "Manage documents")
    //{
    //    Aliases = { "doc" },
    //    Subcommands =
    //    {
    //        documentIngestCommand
    //    },
    //};
    //documentCommand.SetAction((ParseResult parseResult) =>
    //{
    //    Console.WriteLine($"This is the basic document command");
    //});

    //documentCommand.Subcommands.Add(documentIngestCommand);
        

    //Command chatCommand = new("chat", "Start an interactive chat session.");
    //chatCommand.SetAction((ParseResult parseResult, IServiceProvider sp) =>
    //{
    //    Console.WriteLine($"Hello chatters!");
    //    // Here you would typically call your chat service
    //});

    //RootCommand rootCommand = new(ApplicationConstants.ApplicationName)
    //{
    //    //fileOption
    //    Subcommands =
    //    {
    //        chatCommand,
    //        documentCommand
    //    }
    //};

    //Command slashCommand = new("/")
    //{
    //    Description = "Nested command that can be run inside chats.",
    //    Subcommands =
    //    {
    //        documentCommand
    //    }
    //};

    //services.AddKeyedSingleton("SlashCommand", () => slashCommand);
    //services.AddKeyedSingleton("DocCommand", () =>
    //{
    //    Command cmd = new("test")
    //    {
    //        Subcommands =
    //        {
    //            documentIngestCommand
    //        }
    //    };

    //    return cmd;
    //});
    //services.AddSingleton<DocumentServiceCommand>();

    //return rootCommand;

    //var config = new CommandLineConfiguration(rootCommand);
    //return config;
}