using Microsoft.Extensions.Logging;
using SK.Rag.Application.Services.Interfaces;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public class DocumentServiceCommand : Command
        //: base("hello", "Prints a hello world message.")
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentServiceCommand> _logger;

    public DocumentServiceCommand(
        IDocumentService documentService,
        ILogger<DocumentServiceCommand> logger)
        : base("services", "Prints a hello world message.")
    {
        Aliases.Add("svc");

        SetAction(ExecuteAction);
        //{
        //    Console.WriteLine($"This is the document commandHello chatters!");
        //});
    }

    public async Task ExecuteAction(ParseResult context)
    {
        _logger.LogInformation("Executing HelloWithServiceCommand");
        await _documentService.List();
        Console.WriteLine("Hello, World!");
    }
}
