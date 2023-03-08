using System.IO.Abstractions;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.TopDownParse;
using Interpreter.Lib.IR.Ast;
using Interpreter.Services.Providers.LexerProvider.Impl;
using Interpreter.Services.Providers.ParserProvider.Impl;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit.Infrastructure;

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

        var loggingParser = new LoggingParser(parser.Object, "file", _fileSystem.Object);
        var parsed = loggingParser.TopDownParse("");

        _file.Verify(x => x.WriteAllText(
            It.Is<string>(p => p == "ast.dot"),
            It.Is<string>(c => c == "digraph ast { }")
        ), Times.Once());
        Assert.IsType<LoggingAbstractSyntaxTree>(parsed);
    }

    [Fact]
    public void CorrectFileNameProducedByTreeTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.GetInstructions())
            .Returns(new AddressedInstructions { new Halt() });

        _file.Setup(x => x.WriteAllLines(
            It.IsAny<string>(), It.IsAny<IEnumerable<string>>()
        )).Verifiable();

        var loggingTree = new LoggingAbstractSyntaxTree(ast.Object, "file", _fileSystem.Object);
        loggingTree.GetInstructions();

        _file.Verify(x => x.WriteAllLines(
            It.Is<string>(p => p == "file.tac"),
            It.Is<IEnumerable<string>>(c => c.SequenceEqual(new[] { "\tEnd" }))
        ), Times.Once());
    }
}