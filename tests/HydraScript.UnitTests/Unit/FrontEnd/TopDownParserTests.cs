using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Infrastructure;
using HydraScript.UnitTests.TestData;
using Xunit;

namespace HydraScript.UnitTests.Unit.FrontEnd;

public class TopDownParserTests
{
    private readonly IParser _parser = new TopDownParser(new RegexLexer(
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