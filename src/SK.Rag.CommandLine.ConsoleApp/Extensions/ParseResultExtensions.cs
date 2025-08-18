using SK.Rag.CommandLine.ConsoleApp.Commands;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Extensions;
public static class ParseResultExtensions
{
    public static IEnumerable<FileInfo> GetFileList(this ParseResult parseResult)
    {
        if(parseResult is null)        
        {
            return [];
        }

        var dir = parseResult.GetValue(Options.DirectoryOption);
        var file = parseResult.GetValue(Options.FileOption);

        var files = dir?.EnumerateFiles()?.ToList() ?? [];
        if (file is not null)
        {
            files.Add(file);
        }

        return files;
    }
}
