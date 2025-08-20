using Microsoft.Extensions.DependencyInjection;
using SK.Rag.CommandLine.ConsoleApp.Commands.Interfaces;
using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Extensions;

public static class CommandExtensions
{
    public static void SetActionWithServiceScope<TActionHandler>(this Command command, IServiceProvider serviceProvider) where TActionHandler: ICommandActionRunner
    {
        command.SetAction(async (ParseResult parseResult, CancellationToken cancellationToken) =>
        {
            using var serviceScope = serviceProvider.CreateAsyncScope();
            var action = serviceProvider.GetRequiredService<TActionHandler>();
            await action.Run(parseResult, cancellationToken);
        });
    }
}
