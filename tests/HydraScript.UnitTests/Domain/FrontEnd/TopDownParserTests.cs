using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Infrastructure;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class TopDownParserTests
{
    private readonly TopDownParser _parser = new(new RegexLexer(
        new Structure<GeneratedRegexContainer>(new TokenTypesProvider()),
        new TextCoordinateSystemComputer()));

    [Theory]
    [ClassData(typeof(ParserSuccessTestData))]
    public void ParserDoesNotThrowTest(string text)
    {
        var ex = Record.Exception(() =>
        {
            // ReSharper disable once UnusedVariable
            var ast = _parser.Parse(text);
        });
        Assert.Null(ex);
    }
}