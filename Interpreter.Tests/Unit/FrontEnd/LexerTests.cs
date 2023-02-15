using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Services.Providers.StructureProvider.Impl;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit.FrontEnd;

public class LexerTests
{
    private readonly Lexer _lexer;

    public LexerTests()
    {
        _lexer = new Lexer(new StructureProvider().CreateStructure());
    }

    [Theory]
    [ClassData(typeof(LexerSuccessData))]
    public void LexerDoesNotThrowTest(string text) => 
        Assert.Null(Record.Exception(() => _lexer.GetTokens(text)));

    [Theory]
    [ClassData(typeof(LexerFailData))]
    public void LexerThrowsErrorTest(string text) =>
        Assert.Throws<LexerException>(() => _lexer.GetTokens(text));

    [Fact]
    public void LexerToStringCorrectTest()
    {
        const string text = "8";
        var tokens = _lexer.GetTokens(text);
        Assert.Contains("EOP", _lexer.ToString());
        Assert.Equal("IntegerLiteral (1, 1)-(1, 2): 8", tokens.First().ToString());
    }

    [Fact]
    public void EmptyTextTest() => 
        Assert.NotEmpty(_lexer.GetTokens(""));

    [Fact]
    public void GetTokensSkipIgnorableTypesTest()
    {
        const string text = @"
                let x = 1 // int
            ";
        var tokens = _lexer.GetTokens(text);
        Assert.DoesNotContain(_lexer.Structure.FindByTag("Comment"), tokens.Select(x => x.Type));
    }
}