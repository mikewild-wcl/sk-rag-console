using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SK.Rag.CommandLine.ConsoleApp.Commands;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using System.CommandLine;
using System.ComponentModel.DataAnnotations;
using System.Runtime;

// Look into this - https://endjin.com/blog/2020/09/simple-pattern-for-using-system-commandline-with-dependency-injection
// https://learn.microsoft.com/en-us/dotnet/standard/commandline/migration-guide-2.0.0-beta5

var builder = Host.CreateApplicationBuilder();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddLogging(l => l.AddConsole())
    .AddServices()
    .AddClients()
    .AddSemanticKernel();

var rootCommand = BuildRootCommand(builder.Services);

builder.Services.AddSingleton(rootCommand);

var commandLineConfig = new CommandLineConfiguration(rootCommand);

var host = builder.Build();

//Option<FileInfo> fileOption = new("--file")
//{
//    Description = "The file to read and display on the console."
//};

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

var commandResult = await commandLineConfig.InvokeAsync(args);
return commandResult;

//return 1;

static void ReadFile(FileInfo file)
{
    foreach (string line in File.ReadLines(file.FullName))
    {
        Console.WriteLine(line);
    }
}

//static CommandLineConfiguration BuildParser(ServiceProvider serviceProvider)
static RootCommand BuildRootCommand(IServiceCollection services)
{
    services.AddSingleton<ChatCommand>();
    services.AddSingleton<DocumentServiceCommand>();

    var serviceProvider = services.BuildServiceProvider();

    //Command chatCommand = new("chat", "Start an interactive chat session.");
    //chatCommand.SetAction((ParseResult parseResult, IServiceProvider sp) =>
    //{
    //    Console.WriteLine($"Hello chatters!");
    //    // Here you would typically call your chat service
    //});

    /* Try a different way of creating commands with actions */
    Option<DirectoryInfo> directoryOption = new("--dir")
    {
        Aliases = { "-d" },
        Description = "A directory containing files to ingest.",
        AllowMultipleArgumentsPerToken = true
    };

    Option<FileInfo> fileOption = new("--file")
    {        
        Aliases = { "-f" },
        Description = "The file to ingest.",
        AllowMultipleArgumentsPerToken = true,
    };

    Option<FileInfo[]> filesOption = new("--files")
    {
        Description = "A list of files to ingest.",
        AllowMultipleArgumentsPerToken = true,
    };

    Command documentIngestCommand = new("ingest", "Ingest a document")
    {
        directoryOption,
        fileOption
    };

    documentIngestCommand.Validators.Add(parseResult =>
    {
        var dir = parseResult.GetValue(directoryOption);
        var file = parseResult.GetValue(fileOption);

        if(dir is not null && file is not null)
        {
            parseResult.AddError($"Only one of --dir or --file can be used, but not both");
        }

        if (dir is { Exists: false })
        {
            parseResult.AddError($"Directory '{directoryOption.Name}' not found.");
        }

        if (dir is not null && !dir.Exists && dir is { Exists: true })
        {
            parseResult.AddError($"Directory '{directoryOption.Name}' not found.");
        }

        if (file is not null && !file.Exists)
        {
            parseResult.AddError($"File '{fileOption.Name}' not found.");
        }
    });

    documentIngestCommand.SetAction((ParseResult parseResult) =>
    {
        Console.WriteLine($"Ingesting document(s)");
        var dir = parseResult.GetValue(directoryOption);
        var file = parseResult.GetValue(fileOption);
        var files = parseResult.GetValue(filesOption);

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
    //    documentIngestCommand.SetAction((ParseResult parseResult, IServiceProvider sp) =>
    Command documentCommand = new("document")
    {
        Aliases = { "doc" },
        Subcommands =
        {
            documentIngestCommand
        },
    };
    //documentCommand.SetAction((ParseResult parseResult) =>
    //{
    //    Console.WriteLine($"This is the basic document command");
    //});

    //documentCommand.Subcommands.Add(documentIngestCommand);

    RootCommand rootCommand = new("Sample app for System.CommandLine")
    {
        //fileOption
        Subcommands =
        {
            serviceProvider.GetRequiredService<ChatCommand>(),
            serviceProvider.GetRequiredService<DocumentServiceCommand>(),
            documentCommand
            //new DocumentServiceCommand(
            //    serviceProvider.GetRequiredService<IDocumentService>(),
            //    serviceProvider.GetRequiredService<ILogger<DocumentServiceCommand>>())
        }
    };

    return rootCommand;

    //var config = new CommandLineConfiguration(rootCommand);
    //return config;
}