using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SK.Rag.ConsoleApp.Commands;

public class DocumentIngestionCommand : Command<DocumentIngestionCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-p|--path <PATH>")]
        [Description("Ingest a document into the system.")]
        public string? Name { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        foreach (var doc in new string[] { "Document1.doc", "Document2.doc" })
        {
            AnsiConsole.MarkupLine($"Argument: [bold yellow]{doc}[/]");
        }

        return 0;
    }
}
