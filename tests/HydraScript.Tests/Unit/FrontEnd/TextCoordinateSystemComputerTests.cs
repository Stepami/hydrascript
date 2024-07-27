using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Lib.FrontEnd.GetTokens;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

public class TextCoordinateSystemComputerTests
{
    private ITextCoordinateSystemComputer _sut = new TextCoordinateSystemComputer();

    [Fact]
    public void GetLines_NoNewLine_SingleIndexResult()
    {
        const string text = "let x = 0";
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([text.Length + Environment.NewLine.Length - 1]);
    }
    
    [Fact]
    public void GetLines_HasNewLine_SingleIndexResult()
    {
        var text = "let x = 0" + Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([text.Length - 1]);
    }
    
    [Fact]
    public void GetLines_HasNewLines_MultipleIndicesResult()
    {
        const string stmt1 = "let x = 0";
        const string stmt2 = "x = x + 1";
        const string stmt3 = """print("x")""";
        var text = stmt1 +
                   Environment.NewLine +
                   stmt2 +
                   Environment.NewLine +
                   stmt3 +
                   Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo(
        [
            stmt1.Length + Environment.NewLine.Length - 1,
            stmt1.Length + stmt2.Length + Environment.NewLine.Length * 2 - 1,
            stmt1.Length + stmt2.Length + stmt3.Length + Environment.NewLine.Length * 3 - 1
        ]);
    }
}