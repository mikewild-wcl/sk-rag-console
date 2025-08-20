using System.CommandLine;
using SK.Rag.CommandLine.ConsoleApp.Commands;
using SK.Rag.CommandLine.ConsoleApp.Extensions;

namespace SK.Rag.CommandLine.ConsoleApp.UnitTests.Extensions;

public class ParseResultExtensionsTests
{
    [Fact]
    public void GetFileList_WhenParseResultIsNull_ReturnsEmptyList()
    {
        // Arrange
        ParseResult? parseResult = null;

        // Act
        var result = parseResult!.GetFileList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetFileList_WhenNoOptionsProvided_ReturnsEmptyList()
    {
        // Arrange
        var rootCommand = new Command("test")
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        var parseResult = rootCommand.Parse("");

        // Act
        var result = parseResult.GetFileList();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetFileList_WhenOnlyDirectoryProvided_ReturnsDirectoryFiles()
    {
        // Arrange
        var rootCommand = new RootCommand()
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        var testDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        var testFile = Path.Combine(testDir.FullName, "test.txt");
        File.WriteAllText(testFile, "test content");

        var parseResult = rootCommand.Parse($"--dir {testDir.FullName}");

        try
        {
            // Act
            var result = parseResult.GetFileList();

            // Assert
            result.Should().ContainSingle();
            result.First().FullName.Should().Be(testFile);
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDir.FullName, true);
        }
    }

    [Fact]
    public void GetFileList_WhenOnlyFileProvided_ReturnsSingleFile()
    {
        // Arrange
        var rootCommand = new RootCommand()
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        var testFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        File.WriteAllText(testFile, "test content");

        var parseResult = rootCommand.Parse($"--file {testFile}");

        try
        {
            // Act
            var result = parseResult.GetFileList();

            // Assert
            result.Should().ContainSingle();
            result.First().FullName.Should().Be(testFile);
        }
        finally
        {
            // Cleanup
            File.Delete(testFile);
        }
    }

    [Fact]
    public void GetFileList_WhenBothDirectoryAndFileProvided_ReturnsCombinedFiles()
    {
        // Arrange
        var rootCommand = new RootCommand()
        {
            Options.DirectoryOption,
            Options.FileOption
        };

        var testDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));
        var dirTestFile = Path.Combine(testDir.FullName, "test1.txt");
        File.WriteAllText(dirTestFile, "test content 1");

        var singleTestFile = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        File.WriteAllText(singleTestFile, "test content 2");

        var parseResult = rootCommand.Parse($"--dir {testDir.FullName} --file {singleTestFile}");

        try
        {
            // Act
            var result = parseResult.GetFileList().ToList();

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(f => f.FullName == dirTestFile);
            result.Should().Contain(f => f.FullName == singleTestFile);
        }
        finally
        {
            // Cleanup
            Directory.Delete(testDir.FullName, true);
            File.Delete(singleTestFile);
        }
    }
}