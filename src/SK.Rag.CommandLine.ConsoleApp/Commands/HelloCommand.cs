using System.CommandLine;

namespace SK.Rag.CommandLine.ConsoleApp.Commands;
internal class HelloCommand : Command
{
    public HelloCommand()
        : base("hello", "Prints a hello world message.")
    {
        SetAction(ExecuteAction);
        //{
        //    Console.WriteLine($"This is the document commandHello chatters!");
        //});
    }

    public void ExecuteAction(ParseResult context)
    {
        Console.WriteLine("Hello, World!");
    }
}
