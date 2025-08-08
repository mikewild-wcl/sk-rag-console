namespace SK.Rag.CommandLine.ConsoleApp.Commands;

public static class Validators
{
    public static Action<System.CommandLine.Parsing.CommandResult> DocumentOptionsValidator { get; }
        = parseResult =>
        {
            var dir = parseResult.GetValue(Options.DirectoryOption);
            var file = parseResult.GetValue(Options.FileOption);

            //if(dir is not null && file is not null)
            //{
            //    parseResult.AddError($"Only one of --dir or --file can be used, but not both");
            //}

            if (dir is { Exists: false })
            {
                parseResult.AddError($"Directory '{dir.Name}' not found.");
            }

            if (dir is not null && !dir.Exists && dir is { Exists: true })
            {
                parseResult.AddError($"Directory '{dir.Name}' not found.");
            }

            if (file is not null && !file.Exists)
            {
                parseResult.AddError($"File '{file.Name}' not found.");
            }
        };
}
