using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;


Option<FileInfo> fileOption = new("--file")
{
    Description = "The file to read and display on the console."
};

//https://learn.microsoft.com/en-us/dotnet/standard/commandline/migration-guide-2.0.0-beta5
Command chatCommand = new("chat");
chatCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Hello chatters!");
    // Here you would typically call your chat service
}); 

RootCommand rootCommand = new("Sample app for System.CommandLine")
{
    fileOption
};
rootCommand.Subcommands.Add(chatCommand);

rootCommand.SetAction((ParseResult parseResult) =>
{
    Console.WriteLine($"Hello rooters!");
    // Here you would typically call your chat service
});

var parseResult = rootCommand.Parse(args);

if (parseResult.GetValue(fileOption) is FileInfo parsedFile)
{
    ReadFile(parsedFile);
    return 0;
}

var result = await parseResult.InvokeAsync();

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

//var config = new CommandLineConfiguration(rootCommand);
//var commandResult = await config.InvokeAsync(args);

return 1;

static void ReadFile(FileInfo file)
{
    foreach (string line in File.ReadLines(file.FullName))
    {
        Console.WriteLine(line);
    }
}