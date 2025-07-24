using SK.Rag.ConsoleApp.Commands;
using Spectre.Console.Cli;

var app = new CommandApp<InteractiveChatCommand>();

app.Configure(config =>
{
    config.ValidateExamples();

    config.AddCommand<HelloCommand>("hello")
        .WithDescription("Say hello to anyone.")
        .WithExample(new[] { "hello", "--name", "DarthPedro" });

    config.AddCommand<ByeCommand>("bye")
        .WithDescription("Says goodbye.")
        .WithExample(new[] { "bye" });

    config.AddCommand<InteractiveChatCommand>("bye")
        .WithDescription("Chat.");

config.AddBranch("documents", documents =>
{
    documents.SetDescription("ingest, list, or remove documents.");

    documents.AddCommand<DocumentIngestionCommand>("new")
        .WithAlias("add")
        .WithDescription("Ingest documents.")
        .WithExample(new[] { "documents", "ingest", @"c:\mydocs\mydoc.doc", "https://www.website/page.html", "Shakespeare" });
    //config.AddCommand<DocumentIngestionCommand>("bye")
    //    .WithDescription("Ingest documents.")
    //    .WithExample(new[] { @"ingest --path ""C:\mydocs\mydoc.doc""" });

    //student.AddCommand<Doc>("view")
    //    .WithDescription("View student information by id.")
    //    .WithExample(new[] { "student", "view", "1001" });
    });
});

await app.RunAsync(args);

/*
 * Old code from first steps with Spectre.Console
 * Will remove it later.
  
AnsiConsole.Write(
    new FigletText("Rag Console")
        .Centered()
        //.LeftJustified()
        .Color(Color.Aqua));

AnsiConsole.MarkupLine("[red bold]Hello, World[/]");
AnsiConsole.WriteLine("Hello, World!");

AnsiConsole.MarkupLine("[red on white bold]This is the inline markup[/]");

Style danger = new(
    foreground: Color.Red,
    background: Color.White,
    decoration: Decoration.Bold | Decoration.Italic);

AnsiConsole.Write(new Markup("Danger Text from Style", danger));

//Age prompt
var age = await AnsiConsole.PromptAsync(
    new TextPrompt<int>("What is your age?")
        .PromptStyle("blue")
        .DefaultValue(18)
        .ValidationErrorMessage("[red]Age must be a positive number[/]")
        .Validate(x => x switch
        {
            < 0 => ValidationResult.Error("[red]Age must be greater than 0[/]"),
            > 120 =>  ValidationResult.Error("[red]Age must be less than than 120[/]"),
            _ => ValidationResult.Success()
        }));

// add a text prompt
var textPrompt = new TextPrompt<string>("What is your name?")
    .AllowEmpty()
    .DefaultValue("John Doe")
    .ValidationErrorMessage("[red]Name cannot be empty[/]");

var name = await AnsiConsole.PromptAsync(textPrompt);

var happy = await AnsiConsole.PromptAsync(
    new SelectionPrompt<string>()
        .Title("Are you happy?")
        .PageSize(10)        
        .AddChoices(new[] { "Yes", "No", "Maybe" })
        );


AnsiConsole.MarkupLineInterpolated($"Welcome, [bold]{name}[/] aged [bold]{age}[/]. Happy? {happy}");

AnsiConsole.MarkupLine("[slowblink]Press any key to exit[/]");
Console.ReadKey();

AnsiConsole.Clear();
*/