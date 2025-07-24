using Spectre.Console;
using Spectre.Console.Cli;

namespace SK.Rag.ConsoleApp.Commands;

public class InteractiveChatCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.MarkupLine($"Hello, this is the interactive command...");
        return 0;
    }
}
