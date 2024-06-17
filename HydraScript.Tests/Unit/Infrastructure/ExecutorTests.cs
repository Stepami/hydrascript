using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Instructions;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.Ast;
using HydraScript.Services.Executor.Impl;
using HydraScript.Services.Parsing;
using HydraScript.Tests.Helpers;
using HydraScript.Tests.Stubs;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.Infrastructure;

public class ExecutorTests
{
    private readonly Mock<CommandLineSettings> _settings;
    private readonly Mock<IParsingService> _parsingService;

    public ExecutorTests()
    {
        _settings = new Mock<CommandLineSettings>();
        _settings.Setup(x => x.Dump).Returns(false);
        _settings.Setup(x => x.InputFilePath).Returns("file.js");

        _parsingService = new Mock<IParsingService>();
    }

    [Fact]
    public void ExecuteGoesOkTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.GetInstructions())
            .Returns(new AddressedInstructions { new Halt() });

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var executor = new Executor(_parsingService.Object, _settings.ToOptions());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }

    [Fact]
    public void SemanticExceptionCaughtTest()
    {
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.GetInstructions())
            .Throws<SemanticExceptionStub>();

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var executor = new Executor(_parsingService.Object, _settings.ToOptions());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }
        
    [Fact]
    public void LexerExceptionCaughtTest()
    {
        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Throws<LexerException>();

        var executor = new Executor(_parsingService.Object, _settings.ToOptions());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }
        
    [Fact]
    public void ParserExceptionCaughtTest()
    {
        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Throws<ParserException>();

        var executor = new Executor(_parsingService.Object, _settings.ToOptions());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }
        
    [Fact]
    public void InternalInterpreterErrorCaughtTest()
    {
        var instruction = new Mock<Instruction>();
        instruction.Setup(x => x.Execute(It.IsAny<VirtualMachine>()))
            .Throws<NullReferenceException>();
            
        var ast = new Mock<IAbstractSyntaxTree>();
        ast.Setup(x => x.GetInstructions())
            .Returns(new AddressedInstructions { instruction.Object, new Halt() });

        _parsingService.Setup(x => x.Parse(It.IsAny<string>()))
            .Returns(ast.Object);

        var executor = new Executor(_parsingService.Object, _settings.ToOptions());
        Assert.Null(Record.Exception(() => executor.Execute()));
    }
}