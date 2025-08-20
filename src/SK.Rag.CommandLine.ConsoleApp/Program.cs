using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

builder.Build();

//var rootCommand = builder.Services.BuildRootCommand();
//return await rootCommand.Parse(args).InvokeAsync();
return await builder
    .Services
    .BuildRootCommand()
    .Parse(args)
    .InvokeAsync();
