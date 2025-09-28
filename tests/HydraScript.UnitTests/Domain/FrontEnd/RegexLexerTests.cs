using System.Text.RegularExpressions;
using AutoFixture.Xunit3;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class RegexLexerTests(ITestOutputHelper output)
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
        const string text = "let x = 1 // int";
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

    [Theory, AutoHydraScriptData]
    public void GetTokens_MockedRegex_ValidOutput(
        LexerInput input,
        [Frozen] IStructure structure,
        RegexLexer lexer)
    {
        output.WriteLine(input.ToString());
        var patterns = TokenInput.Pattern.Split('|');

        structure.Regex.ReturnsForAnyArgs(
            new Regex(string.Join('|', patterns.Select((x, i) => $"(?<TYPE{i}>{x})"))));
        var tokenTypes = Enumerable.Range(0, patterns.Length)
            .Select(x => new TokenType($"TYPE{x}"))
            .ToList();

        structure.Count.Returns(tokenTypes.Count);
        structure[Arg.Any<int>()].Returns(callInfo => tokenTypes[callInfo.Arg<int>()]);

        var tokens = lexer.GetTokens(input.ToString());
        for (var i = 0; i < input.Count; i++)
        {
            output.WriteLine(tokens[i].ToString());
            tokens[i].Value.Should().BeEquivalentTo(input[i]);
            tokens[i].Type.Should().BeOneOf(tokenTypes);
        }
    }
}