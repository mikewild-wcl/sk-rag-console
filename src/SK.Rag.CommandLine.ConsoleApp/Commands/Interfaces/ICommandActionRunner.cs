using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
public interface ICommandActionRunner
{
    Task Run(ParseResult parseResult, CancellationToken cancellationToken);
}
