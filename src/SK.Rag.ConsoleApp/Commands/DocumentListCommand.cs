using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SK.Rag.ConsoleApp.Commands;

public class DocumentListCommand : Command
{
    [Description("List documents that have been ingested into the system.")]
    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Document1.doc");
        AnsiConsole.WriteLine("Document2.doc");
        AnsiConsole.WriteLine("Document3.doc");

        // Placeholder for document listing logic
        return 0;
    }
}
