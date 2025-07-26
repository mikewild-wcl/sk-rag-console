using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SK.Rag.Application.Configuration;
using SK.Rag.Application.Services;
using SK.Rag.Application.Services.Interfaces;
using SK.Rag.CommandLine.ConsoleApp.Commands;
using SK.Rag.CommandLine.ConsoleApp.Extensions;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

// Look into this - https://endjin.com/blog/2020/09/simple-pattern-for-using-system-commandline-with-dependency-injection
// https://learn.microsoft.com/en-us/dotnet/standard/commandline/migration-guide-2.0.0-beta5

var builder = Host.CreateApplicationBuilder();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddLogging(l => l.AddConsole())
    .AddServices()
    .AddClients()
    .AddSemanticKernel();

var config = BuildParser(builder.Services);

var host = builder.Build();

Option<FileInfo> fileOption = new("--file")
{
    Description = "The file to read and display on the console."
};

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
rootCommand.Subcommands.Add(new HelloCommand());

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


var commandResult = await config.InvokeAsync(args);

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
static CommandLineConfiguration BuildParser(IServiceCollection services)
{
    services.AddScoped<DocumentServiceCommand>();

    var serviceProvider = services.BuildServiceProvider();

    RootCommand rootCommand = new("Sample app for System.CommandLine")
    {
        //fileOption
        Subcommands =
        {
            new HelloCommand(),
            serviceProvider.GetRequiredService<DocumentServiceCommand>()
            //new DocumentServiceCommand(
            //    serviceProvider.GetRequiredService<IDocumentService>(),
            //    serviceProvider.GetRequiredService<ILogger<DocumentServiceCommand>>())
        }
    };

    var config = new CommandLineConfiguration(rootCommand);

    return config;
}