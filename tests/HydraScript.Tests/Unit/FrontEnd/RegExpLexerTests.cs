using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Services.Providers.StructureProvider.Impl;
using HydraScript.Tests.TestData;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

public class RegExpLexerTests
{
    private readonly RegExpLexer _regExpLexer = new(
        new StructureProvider().CreateStructure(),
        new TextCoordinateSystemComputer());

    [Theory]
    [ClassData(typeof(LexerSuccessData))]
    public void LexerDoesNotThrowTest(string text) => 
        Assert.Null(Record.Exception(() => _regExpLexer.GetTokens(text)));

    [Theory]
    [ClassData(typeof(LexerFailData))]
    public void LexerThrowsErrorTest(string text) =>
        Assert.Throws<LexerException>(() => _regExpLexer.GetTokens(text));

    [Fact]
    public void LexerToStringCorrectTest()
    {
        const string text = "8";
        var tokens = _regExpLexer.GetTokens(text);
        Assert.Contains("EOP", _regExpLexer.ToString());
        Assert.Equal("IntegerLiteral (1, 1)-(1, 2): 8", tokens.First().ToString());
    }

    [Fact]
    public void EmptyTextTest() => 
        Assert.NotEmpty(_regExpLexer.GetTokens(""));

    [Fact]
    public void GetTokensSkipIgnorableTypesTest()
    {
        const string text = @"
                let x = 1 // int
            ";
        var tokens = _regExpLexer.GetTokens(text);
        Assert.DoesNotContain(_regExpLexer.Structure.FindByTag("Comment"), tokens.Select(x => x.Type));
    }
}