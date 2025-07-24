using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;

namespace SK.Rag.ConsoleApp.Commands;

// https://darthpedro.net/2020/12/10/lesson-1-1-starting-with-spectre-cli/
internal class HelloCommand : Command<HelloCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name <NAME>")]
        [Description("The person or thing to greet.")]
        [DefaultValue("World")]
        public string? Name { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"Hello [bold yellow]{settings.Name}[/]!");
        return 0;
    }
}
