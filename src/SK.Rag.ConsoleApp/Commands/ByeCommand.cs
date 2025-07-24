using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Rag.ConsoleApp.Commands;

public class ByeCommand : Command
{
    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Okay, Byeeee!!!");
        return 0;
    }
}
