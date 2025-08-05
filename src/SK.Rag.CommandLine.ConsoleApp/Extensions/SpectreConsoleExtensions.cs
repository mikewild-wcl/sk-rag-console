using Spectre.Console;

namespace SK.Rag.CommandLine.ConsoleApp.Extensions;
public static class SpectreConsoleExtensions
{
    public const string ApplicationDisplayName = "RAG Console";

    public static void WriteApplicationFigletText(this IAnsiConsole console)
    {
        console.WriteFigletText(ApplicationDisplayName);
    }

    public static void WriteFigletText(this IAnsiConsole console, string text)
    {
        console.Write(new FigletText(text)
            .Centered()
            .Color(Color.SteelBlue));
    }

    public static void WriteLineWithColor(this IAnsiConsole console, string message, Color color)
    {
        console.MarkupLine($"[bold {color}]{message}[/]");
    }

    public static void WriteError(this IAnsiConsole console, string message)
    {
        console.WriteLineWithColor(message, Color.Red);
    }

    public static void WriteSuccess(this IAnsiConsole console, string message)
    {
        console.WriteLineWithColor(message, Color.Green);
    }
}
