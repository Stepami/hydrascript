using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.TopDownParse;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;
using Interpreter.Services.Providers.StructureProvider.Impl;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit.FrontEnd;

public class ParserTests
{
    private readonly IParser _parser;

    public ParserTests()
    {
        _parser = new Parser(new Lexer(
            new StructureProvider()
                .CreateStructure()
        ));
    }

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