using System.IO.Abstractions;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.FrontEnd.GetTokens.Data.TokenTypes;
using HydraScript.Lib.FrontEnd.GetTokens.Impl;
using HydraScript.Lib.FrontEnd.TopDownParse.Impl;
using HydraScript.Services.Providers.LexerProvider;
using HydraScript.Services.Providers.LexerProvider.Impl;
using HydraScript.Services.Providers.ParserProvider.Impl;
using HydraScript.Services.Providers.StructureProvider;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using SystemType = System.Type;

namespace HydraScript.Tests.Unit.Infrastructure;

public class ProvidersTests
{
    [Theory]
    [InlineData(typeof(Lexer), false)]
    [InlineData(typeof(LoggingLexer), true)]
    public void CertainLexerProvidedTest(SystemType lexerType, bool dump)
    {
        var structureProvider = new Mock<IStructureProvider>();
        structureProvider.Setup(x => x.CreateStructure())
            .Returns(new Structure(new List<TokenType>()));

        var options = new Mock<IOptions<CommandLineSettings>>();
        options.Setup(x => x.Value)
            .Returns(new CommandLineSettings
            {
                Dump = dump,
                InputFilePath = "file.js"
            });

        var lexerProvider = new LexerProvider(
            structureProvider.Object,
            Mock.Of<ITextCoordinateSystemComputer>(),
            Mock.Of<IFileSystem>(),
            options.Object);
        var lexer = lexerProvider.CreateLexer();

        Assert.IsType(lexerType, lexer);
    }

    [Theory]
    [InlineData(typeof(Parser), false)]
    [InlineData(typeof(LoggingParser), true)]
    public void CertainParserProvidedTest(SystemType parserType, bool dump)
    {
        var options = new Mock<IOptions<CommandLineSettings>>();
        options.Setup(x => x.Value)
            .Returns(new CommandLineSettings
            {
                Dump = dump,
                InputFilePath = "file.js"
            });

        var lexer = new Mock<ILexer>();
        var lexerProvider = new Mock<ILexerProvider>();
        lexerProvider.Setup(x => x.CreateLexer())
            .Returns(lexer.Object);

        var parserProvider = new ParserProvider(lexerProvider.Object, Mock.Of<IFileSystem>(), options.Object);
        var parser = parserProvider.CreateParser();

        Assert.IsType(parserType, parser);
    }
}