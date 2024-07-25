using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Impl;
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
        result.Should().BeEquivalentTo([10]);
    }
    
    [Fact]
    public void GetLines_HasNewLine_SingleIndexResult()
    {
        var text = "let x = 0" + Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([10]);
    }
    
    [Fact]
    public void GetLines_HasNewLines_MultipleIndicesResult()
    {
        var text = "let x = 0" +
                   Environment.NewLine +
                   "x = x + 1" +
                   Environment.NewLine +
                   """print("x")""" +
                   Environment.NewLine;
        var result = _sut.GetLines(text);
        result.Should().BeEquivalentTo([10, 21, 33]);
    }
}