using HydraScript.Lib.FrontEnd.GetTokens.Impl;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.FrontEnd.TopDownParse.Impl;
using HydraScript.Services.Providers.StructureProvider.Impl;
using HydraScript.Tests.TestData;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

public class ParserTests
{
    private readonly IParser _parser = new Parser(new Lexer(
        new StructureProvider()
            .CreateStructure(),
        new TextCoordinateSystemComputer()));

    [Theory]
    [ClassData(typeof(ParserSuccessTestData))]
    public void ParserDoesNotThrowTest(string text)
    {
        var ex = Record.Exception(() =>
        {
            // ReSharper disable once UnusedVariable
            var ast = _parser.TopDownParse(text);
        });
        Assert.Null(ex);
    }
}