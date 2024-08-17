using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Infrastructure;
using HydraScript.Infrastructure.Dumping;
using Microsoft.Extensions.Options;
using Moq;
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
            .Returns([]);
        lexer.Setup(x => x.ToString())
            .Returns("lexer");

        _file.Setup(
                x => x.WriteAllText(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .Verifiable();

        var loggingLexer = new DumpingLexer(
            lexer.Object,
            _fileSystem.Object,
            inputFile: Options.Create(new InputFile { Info = new FileInfo("file") }));
        loggingLexer.GetTokens("");

        _file.Verify(
            x => x.WriteAllText(
                It.Is<string>(p => p == "file.tokens"),
                It.Is<string>(c => c == "lexer")),
            Times.Once());
    }

    [Fact]
    public void CorrectTreeWrittenAndLoggingTreeProducedTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.ToString())
            .Returns("digraph ast { }");

        var parser = new Mock<IParser>();
        parser.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        _file.Setup(x => x.WriteAllText(
            It.IsAny<string>(), It.IsAny<string>()
        )).Verifiable();

        var loggingParser = new DumpingParser(parser.Object, _fileSystem.Object);
        _ = loggingParser.Parse("");

        _file.Verify(
            x => x.WriteAllText(
                It.Is<string>(p => p == "ast.dot"),
                It.Is<string>(c => c == "digraph ast { }")),
            Times.Once());
    }
}