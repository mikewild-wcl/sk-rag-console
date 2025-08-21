using SK.Rag.CommandLine.ConsoleApp.Extensions;
using Spectre.Console;
using System.Reflection;

namespace SK.Rag.CommandLine.ConsoleApp.UnitTests.Extensions;

public class SpectreConsoleExtensionsTests
{ 
    private readonly FieldInfo _textField;

    public SpectreConsoleExtensionsTests()
    {
        _textField = typeof(FigletText)
            .GetField("_text", BindingFlags.NonPublic | BindingFlags.Instance) // Use reflection to get the private _text field value
            ?? throw new InvalidOperationException("Could not find _text field in FigletText");
    }

    [Fact]
    public void WriteApplicationFigletText_CallsWriteFigletTextWithAppName()
    {
        // Arrange
        var mockConsole = new Mock<IAnsiConsole>();
        mockConsole.Setup(c => c.Write(It.IsAny<FigletText>()))
            .Verifiable();

        // Act
        mockConsole.Object.WriteApplicationFigletText();

        // Assert
        mockConsole.Verify(c => c.Write(It.IsAny<FigletText>()), Times.Once);
        mockConsole.Verify(c => c.Write(It.Is<FigletText>(f => f.Color == Color.SteelBlue)), Times.Once);
        mockConsole.Verify(c => c.Write(It.Is<FigletText>(f => f.Justification == Justify.Center)), Times.Once);
        mockConsole.Verify(c => c.Write(It.Is<FigletText>(f => GetFigletText(f) == ApplicationConstants.ApplicationName)), Times.Once);
    }

    [Fact]
    public void WriteFigletText_CallsWriteWithCorrectFigletText()
    {
        var mockConsole = new Mock<IAnsiConsole>();
        var testText = "Hello";
        mockConsole.Setup(c => c.Write(It.IsAny<FigletText>())).Verifiable();

        mockConsole.Object.WriteFigletText(testText);

        mockConsole.Verify(c => c.Write(It.Is<FigletText>(f => GetFigletText(f) == testText)), Times.Once);
    }
        
    private string GetFigletText(FigletText figlet) =>
        _textField.GetValue(figlet)?.ToString() ?? string.Empty;
}
