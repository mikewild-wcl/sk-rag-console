using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;
public static class Options
{
    public static Option<DirectoryInfo> DirectoryOption { get; } 
        = new("--dir")
        {
            Aliases = { "-d" },
            Description = "A directory containing files to ingest.",
            AllowMultipleArgumentsPerToken = true
        };

    public static Option<FileInfo> FileOption { get; }
        = new("--file")
        {
            Aliases = { "-f" },
            Description = "The file to ingest.",
            AllowMultipleArgumentsPerToken = true,
        };

    public static Option<FileInfo[]> FilesOption { get; } 
        = new("--files")
        {
            Description = "A list of files to ingest.",
            AllowMultipleArgumentsPerToken = true,
        };
}
