using Spectre.Console;

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
