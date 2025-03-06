using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using AutoFixture.Xunit2;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using HydraScript.Infrastructure;

namespace HydraScript.UnitTests.Domain.FrontEnd;

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

    [Theory, AutoData]
    public void GetTokens_MockedRegex_ValidOutput([MinLength(10), MaxLength(25)] TokenInput[] tokenInputs)
    {
        var patterns = TokenInput.Pattern.Split('|');

        var structure = Substitute.For<IStructure>();
        var lexer = new RegexLexer(structure, new TextCoordinateSystemComputer());
        structure.Regex.ReturnsForAnyArgs(
            new Regex(string.Join('|', patterns.Select((x, i) => $"(?<TYPE{i}>{x})"))));
        var tokenTypes = Enumerable.Range(0, patterns.Length)
            .Select(x => new TokenType($"TYPE{x}"))
            .ToList();

        // ReSharper disable once GenericEnumeratorNotDisposed
        structure.GetEnumerator()
            .ReturnsForAnyArgs(_ => tokenTypes.GetEnumerator());

        var tokens = lexer.GetTokens(
            tokenInputs.Aggregate(
                TokenInput.AdditiveIdentity,
                (x, y) => x + y).Value);
        for (var i = 0; i < tokenInputs.Length; i++)
        {
            tokens[i].Value.Should().BeEquivalentTo(tokenInputs[i].Value);
            tokens[i].Type.Should().BeOneOf(tokenTypes);
        }
    }
}