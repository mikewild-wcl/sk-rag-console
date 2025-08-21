using Spectre.Console;

namespace SK.Rag.CommandLine.ConsoleApp.Extensions;

public static class SpectreConsoleExtensions
{
    public static void WriteApplicationFigletText(this IAnsiConsole console)
    {
        console.WriteFigletText(ApplicationConstants.ApplicationName);
    }

    public static void WriteFigletText(this IAnsiConsole console, string text)
    {
        console.Write(new FigletText(text)
            .Centered()
            .Color(Color.SteelBlue));
    }
}
