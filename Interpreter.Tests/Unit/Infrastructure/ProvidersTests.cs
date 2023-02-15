using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Data.TokenTypes;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;
using Interpreter.Services.Providers.LexerProvider;
using Interpreter.Services.Providers.LexerProvider.Impl;
using Interpreter.Services.Providers.ParserProvider.Impl;
using Interpreter.Services.Providers.StructureProvider;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using SystemType = System.Type;

namespace Interpreter.Tests.Unit.Infrastructure;

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
            
        var lexerProvider = new LexerProvider(structureProvider.Object, options.Object);
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

        var parserProvider = new ParserProvider(lexerProvider.Object, options.Object);
        var parser = parserProvider.CreateParser();
            
        Assert.IsType(parserType, parser);
    }
}