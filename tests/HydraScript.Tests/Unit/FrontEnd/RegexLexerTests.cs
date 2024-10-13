using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using HydraScript.Infrastructure;
using HydraScript.Tests.TestData;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

public class RegexLexerTests
{
    private readonly RegexLexer _regexLexer = new(
        new Structure<GeneratedRegexContainer>(new TokenTypesProvider()),
        new TextCoordinateSystemComputer());

    [Theory]
    [ClassData(typeof(LexerSuccessData))]
    public void LexerDoesNotThrowTest(string text) => 
        Assert.Null(Record.Exception(() => _regexLexer.GetTokens(text)));

    [Theory]
    [ClassData(typeof(LexerFailData))]
    public void LexerThrowsErrorTest(string text) =>
        Assert.Throws<LexerException>(() => _regexLexer.GetTokens(text));

    [Fact]
    public void LexerToStringCorrectTest()
    {
        const string text = "8";
        var tokens = _regexLexer.GetTokens(text);
        Assert.Contains("EOP", _regexLexer.ToString());
        Assert.Equal("IntegerLiteral (1, 1)-(1, 2): 8", tokens.First().ToString());
    }

    [Fact]
    public void EmptyTextTest() => 
        Assert.NotEmpty(_regexLexer.GetTokens(""));

    [Fact]
    public void GetTokensSkipIgnorableTypesTest()
    {
        const string text = @"
                let x = 1 // int
            ";
        var tokens = _regexLexer.GetTokens(text);
        Assert.DoesNotContain(_regexLexer.Structure.FindByTag("Comment"), tokens.Select(x => x.Type));
    }

    [Theory, ClassData(typeof(LexerKeywordInsideIdentData))]
    public void GetTokens_KeywordInsideIdent_Ident(string input)
    {
        var tokens = _regexLexer.GetTokens(input);
        var token = tokens.First();
        token.Type.Should().Be(new TokenType("Ident"));
    }
}