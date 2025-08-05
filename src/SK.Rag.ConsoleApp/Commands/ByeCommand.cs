using Spectre.Console;
using Spectre.Console.Cli;

namespace SK.Rag.ConsoleApp.Commands;

public class ByeCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Okay, Byeeee!!!");
        return 0;
    }
}
