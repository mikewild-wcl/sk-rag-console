# sk-rag-console

A console project using Azure Open AI and Semantic Kernel to ingest documents and use them for RAG queries. 
The project will also use Specter Console to provide a user interface for interacting with the RAG system.
Will include a custom SQL connector for vector data.


## Spectre Console and System.CommandLine

Spectre Console is a library for building beautiful command line applications in .NET.

Links to check:
  https://anthonysimmon.com/true-dependency-injection-with-system-commandline/
  https://anthonysimmon.com/beautiful-interactive-console-apps-with-system-commandline-and-spectre-console/

  DarthPedro's [Blog](https://darthpedro.net/category/spectre-console/) on Spectre.Console,  
  and [Extensions for common code and patterns when using Spectre.Console CLI app framework](https://github.com/d20Tek/Spectre.Console.Extensions).

  Set up an interactive command app - https://darthpedro.net/2024/07/01/creating-interactive-console-applications-in-c/.


## Running console apps in Powershell (Windows Terminal)

Trick to avid having to type `.\`, do this first: 
```powershell
$env:PATH =$env:PATH+";."
```

Set alias temporarily - 
```powershell
New-Alias ragc .\ragconsole
New-Alias ragconsole "<path-start>\sk-rag-console\src\SK.Rag.ConsoleApp\bin\Debug\net9.0\ragconsole.exe"
```

From Developer console in Visual Studio:
```
New-Alias ragconsole ".\SK.Rag.ConsoleApp\bin\Debug\net9.0\ragconsole.exe"
New-Alias ragcommand ".\SK.Rag.CommandLine.ConsoleApp\bin\Debug\net9.0\ragcommand.exe"
```

Test a command by running in Developer Powershell (from the src directory):
```powershell
ragcommand -- --file bin/Debug/net9.0/scl.runtimeconfig.json
ragcommand --file SK.Rag.CommandLine.ConsoleApp\bin\Debug\net9.0\ragcommand.runtimeconfig.json
```



## Unit tests

Using the new xunit v3. See [xUnit v3 - what's new](https://xunit.net/docs/getting-started/v3/whats-new).


