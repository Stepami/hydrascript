using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.Ast;
using HydraScript.Services.Parsing.Impl;
using HydraScript.Services.Providers.ParserProvider;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.Infrastructure;

public class ParsingServiceTests
{
    [Fact]
    public void CertainTextHasBeenParsedTest()
    {
        const string text = "let x = 1 + 2 - 3";

        var ast = new Mock<IAbstractSyntaxTree>();
        var parser = new Mock<IParser>();
        parser.Setup(x => x.TopDownParse(It.IsAny<string>()))
            .Returns(ast.Object).Verifiable();
            
        var parserProvider = new Mock<IParserProvider>();
        parserProvider.Setup(x => x.CreateParser())
            .Returns(parser.Object);

        var parsingService = new ParsingService(parserProvider.Object);
        parsingService.Parse(text);

        parser.Verify(x => x.TopDownParse(
            It.Is<string>(s => s == text)
        ), Times.Once());
    }
}