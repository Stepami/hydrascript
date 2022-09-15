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
        var vm = new VirtualMachine();
        var program = new List<Instruction>();
        Assert.Throws<ArgumentOutOfRangeException>(() => vm.Run(program));
        
        program.Add(new Halt(0));
        Assert.Null(Record.Exception(() => vm.Run(program)));
    }

    [Fact]
    public void VirtualMachineFramesClearedAfterExecutionTest()
    {
        var vm = new VirtualMachine();
        var program = new List<Instruction>()
        {
            new Simple("a", (new Constant(1, "1"), new Constant(2, "2")), "+", 0),
            new AsString("b", new Name("a"), 1),
            new Halt(2)
        };
        
        vm.Run(program);
        Assert.Empty(vm.Frames);
    }
}