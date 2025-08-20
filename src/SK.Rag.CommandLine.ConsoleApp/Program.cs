using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SK.Rag.CommandLine.ConsoleApp.Extensions;

var builder = Host.CreateApplicationBuilder();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddLogging(l => l.AddConsole())
    .AddServices()
    .AddClients()
    .AddSemanticKernel();

builder.Build();

return await builder
    .Services
    .BuildRootCommand()
    .Parse(args)
    .InvokeAsync();
