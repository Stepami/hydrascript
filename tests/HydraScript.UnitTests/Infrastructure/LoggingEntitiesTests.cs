using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Infrastructure;
using HydraScript.Infrastructure.Dumping;
using Microsoft.Extensions.Options;

namespace HydraScript.UnitTests.Infrastructure;

public class LoggingEntitiesTests
{
    private readonly IFile _file;
    private readonly IFileSystem _fileSystem;

    public LoggingEntitiesTests()
    {
        _file = Substitute.For<IFile>();

        _fileSystem = Substitute.For<IFileSystem>();
        _fileSystem.File.Returns(_file);
    }

    [Fact]
    public void CorrectFileNameProducedByLexerTest()
    {
        var lexer = Substitute.For<ILexer>();
        lexer.GetTokens(default!).ReturnsForAnyArgs([]);

        var inputFile = Options.Create(new InputFile { Info = new FileInfo("file") });
        var loggingLexer = new DumpingLexer(lexer, _fileSystem, inputFile);
        loggingLexer.GetTokens("");

        _file.Received(1).WriteAllText(
            Arg.Is<string>(p => p == "file.tokens"),
            Arg.Is<string>(c => c == lexer.ToString()));
    }

    [Fact]
    public void CorrectTreeWrittenAndLoggingTreeProducedTest()
    {
        var ast = Substitute.For<IAbstractSyntaxTree>();

        var parser = Substitute.For<IParser>();
        parser.Parse(default!).ReturnsForAnyArgs(ast);

        var loggingParser = new DumpingParser(parser, _fileSystem);
        _ = loggingParser.Parse("");

        _file.Received(1).WriteAllText(
            Arg.Is<string>(p => p == "ast.dot"),
            Arg.Is<string>(c => c == ast.ToString()));
    }
}