using System.IO.Abstractions;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Data;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.Ast;
using HydraScript.Services.Providers.LexerProvider.Impl;
using HydraScript.Services.Providers.ParserProvider.Impl;
using Xunit;

namespace HydraScript.Tests.Unit.Infrastructure;

public class LoggingEntitiesTests
{
    private readonly Mock<IFile> _file;
    private readonly Mock<IFileSystem> _fileSystem;

    public LoggingEntitiesTests()
    {
        _file = new Mock<IFile>();

        _fileSystem = new Mock<IFileSystem>();
        _fileSystem.Setup(x => x.File)
            .Returns(_file.Object);
    }

    [Fact]
    public void CorrectFileNameProducedByLexerTest()
    {
        var lexer = new Mock<ILexer>();
        lexer.Setup(x => x.GetTokens(It.IsAny<string>()))
            .Returns(new List<Token>());
        lexer.Setup(x => x.ToString())
            .Returns("lexer");

        _file.Setup(x => x.WriteAllText(
            It.IsAny<string>(), It.IsAny<string>()
        )).Verifiable();

        var loggingLexer = new LoggingLexer(lexer.Object, "file", _fileSystem.Object);
        loggingLexer.GetTokens("");

        _file.Verify(x => x.WriteAllText(
            It.Is<string>(p => p == "file.tokens"), It.Is<string>(c => c == "lexer")
        ), Times.Once());
    }

    [Fact]
    public void CorrectTreeWrittenAndLoggingTreeProducedTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.ToString())
            .Returns("digraph ast { }");

        var parser = new Mock<IParser>();
        parser.Setup(x => x.TopDownParse(It.IsAny<string>()))
            .Returns(ast.Object);

        _file.Setup(x => x.WriteAllText(
            It.IsAny<string>(), It.IsAny<string>()
        )).Verifiable();

        var loggingParser = new LoggingParser(parser.Object, _fileSystem.Object);
        _ = loggingParser.TopDownParse("");

        _file.Verify(x => x.WriteAllText(
            It.Is<string>(p => p == "ast.dot"),
            It.Is<string>(c => c == "digraph ast { }")
        ), Times.Once());
    }
}