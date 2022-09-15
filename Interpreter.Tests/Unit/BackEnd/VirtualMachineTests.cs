#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd;

public class VirtualMachineTests
{
    private readonly VirtualMachine _vm;

    public VirtualMachineTests()
    {
        _vm = new(new(), new(), new(), TextWriter.Null);
    }
    
    [Fact]
    public void CorrectPrintToOutTest()
    {
        var writer = new Mock<TextWriter>();
        writer.Setup(x => x.WriteLine(It.IsAny<object?>()))
            .Verifiable();

        var vm = new VirtualMachine(new(), new Stack<Frame>(new[] { new Frame() }), new(), writer.Object);
        var print = new Print(0, new Constant(223, "223"));

        print.Execute(vm);
        writer.Verify(x => x.WriteLine(
            It.Is<object?>(v => v!.Equals(223))
        ), Times.Once());
    }

    [Fact]
    public void ProgramWithoutHaltWillNotRunTest()
    {
        var program = new List<Instruction>();
        Assert.Throws<ArgumentOutOfRangeException>(() => _vm.Run(program));
        
        program.Add(new Halt(0));
        Assert.Null(Record.Exception(() => _vm.Run(program)));
    }

    [Fact]
    public void VirtualMachineFramesClearedAfterExecutionTest()
    {
        var program = new List<Instruction>()
        {
            new Simple("a", (new Constant(1, "1"), new Constant(2, "2")), "+", 0),
            new AsString("b", new Name("a"), 1),
            new Halt(2)
        };
        
        _vm.Run(program);
        Assert.Empty(_vm.Frames);
    }
}