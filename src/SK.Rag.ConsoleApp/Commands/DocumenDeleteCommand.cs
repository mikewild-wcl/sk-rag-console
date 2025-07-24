using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SK.Rag.ConsoleApp.Commands;

public class DocumentDeleteCommand : Command<DocumentDeleteCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-p|--path <PATH>")]
        [Description("Ingest a document into the system.")]
        public string? Name { get; set; }
    }

    [Description("Delete documents that have been ingested into the system.")]
    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.WriteLine("Deleting documentsDocument3.doc");

        // Placeholder for document listing logic
        return 0;
    }
}
