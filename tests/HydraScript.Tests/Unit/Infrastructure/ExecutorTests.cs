using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.IR.Ast;
using HydraScript.Services.CodeGen;
using HydraScript.Services.Executor.Impl;
using HydraScript.Services.Parsing;
using HydraScript.Services.SourceCode;
using HydraScript.Tests.Stubs;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.Infrastructure;

public class ExecutorTests
{
    private readonly Mock<IParsingService> _parsingService;

    public ExecutorTests()
    {
        _parsingService = new Mock<IParsingService>();
    }

    [Fact]
    public void ExecuteGoesOkTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.Root)
            .Returns(Mock.Of<IAbstractSyntaxTreeNode>());

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var codeGenService = new Mock<ICodeGenService>();
        codeGenService.Setup(x => x.GetInstructions(It.IsAny<IAbstractSyntaxTree>()))
            .Returns([new Halt()]);

        var executor = new Executor(
            _parsingService.Object,
            Mock.Of<ISourceCodeProvider>(),
            codeGenService.Object);
        Assert.Null(Record.Exception(() => executor.Execute()));
    }

    [Fact]
    public void SemanticExceptionCaughtTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.Root)
            .Returns(Mock.Of<IAbstractSyntaxTreeNode>());

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var codeGenService = new Mock<ICodeGenService>();
        codeGenService.Setup(x => x.GetInstructions(It.IsAny<IAbstractSyntaxTree>()))
            .Throws<SemanticExceptionStub>();

        var executor = new Executor(
            _parsingService.Object,
            Mock.Of<ISourceCodeProvider>(),
            codeGenService.Object);
        Assert.Null(Record.Exception(() => executor.Execute()));
    }

    [Fact]
    public void LexerExceptionCaughtTest()
    {
        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Throws<LexerException>();

        var executor = new Executor(
            _parsingService.Object,
            Mock.Of<ISourceCodeProvider>(),
            Mock.Of<ICodeGenService>());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }

    [Fact]
    public void ParserExceptionCaughtTest()
    {
        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Throws<ParserException>();

        var executor = new Executor(
            _parsingService.Object,
            Mock.Of<ISourceCodeProvider>(),
            Mock.Of<ICodeGenService>());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }

    [Fact]
    public void InternalInterpreterErrorCaughtTest()
    {
        var instruction = new Mock<IExecutableInstruction>();
        instruction.Setup(x => x.Execute(It.IsAny<IExecuteParams>()))
            .Throws<NullReferenceException>();

        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.Root)
            .Returns(Mock.Of<IAbstractSyntaxTreeNode>());

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var codeGenService = new Mock<ICodeGenService>();
        codeGenService.Setup(x => x.GetInstructions(It.IsAny<IAbstractSyntaxTree>()))
            .Returns([instruction.Object, new Halt()]);

        var executor = new Executor(
            _parsingService.Object,
            Mock.Of<ISourceCodeProvider>(),
            codeGenService.Object);
        Assert.Null(Record.Exception(() => executor.Execute()));
    }
}