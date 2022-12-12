#nullable enable
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

    [Fact]
    public void VirtualMachineHandlesRecursionTest()
    {
        var halt = new Mock<Halt>(12).Trackable();
        var factorial = new FunctionInfo("fact", 1);
        var program = new List<Instruction>
        {
            new Goto(10, 0),
            new BeginFunction(1, factorial),
            new Simple("_t2", (new Name("n"), new Constant(2, "2")), "<", 2),
            new IfNotGoto(new Name("_t2"), 5, 3),
            new Return(1, 4, new Name("n")),
            new Simple("_t5", (new Name("n"), new Constant(1, "1")), "-", 5),
            new PushParameter(6, "n", new Name("_t5")),
            new CallFunction(factorial, 7, 1, "f"),
            new Simple("_t8", (new Name("n"), new Name("f")), "*", 8),
            new Return(1, 9, new Name("_t8")),
            new PushParameter(10, "n", new Constant(6, "6")),
            new CallFunction(factorial, 11, 1, "fa6"),
            halt.Object
        };
        
        _vm.Run(program);
        Assert.Empty(_vm.CallStack);
        Assert.Empty(_vm.Arguments);
        halt.Verify(x => x.Execute(
            It.Is<VirtualMachine>(
                vm => Convert.ToInt32(vm.Frames.Peek()["fa6"]) == 720
            )
        ), Times.Once());
        _vm.Frames.Pop();
    }

    [Fact]
    public void CreateArrayReservesCertainSpaceTest()
    {
        var vm = new VirtualMachine();
        vm.Frames.Push(new Frame());
            
        var createArray = new CreateArray(0, "arr", 6);
        createArray.Execute(vm);
        Assert.Equal(6, ((List<object>) vm.Frames.Peek()["arr"]).Count);

        var indexAssignment = new IndexAssignment("arr", (new Constant(0, "0"), new Constant(0, "0")), 1);
        indexAssignment.Execute(vm);
        Assert.Equal(0, ((List<object>) vm.Frames.Peek()["arr"])[0]);

        var removeFromArray = new RemoveFromArray(2, "arr", new Constant(5, "5"));
        removeFromArray.Execute(vm);
        Assert.Equal(5, ((List<object>) vm.Frames.Peek()["arr"]).Count);
    }

    [Fact]
    public void ObjectCreationTest()
    {
        var halt = new Mock<Halt>(2).Trackable();
        var program = new List<Instruction>
        {
            new CreateObject(0, "obj"),
            new DotAssignment("obj", (new Constant("prop", "prop"), new Constant(null, "null")), 1),
            halt.Object
        };
            
        _vm.Run(program);
        halt.Verify(x => x.Execute(
            It.Is<VirtualMachine>(
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                vm => ((Dictionary<string, object>)vm.Frames.Peek()["obj"])["prop"] == null
            )
        ), Times.Once());
        _vm.Frames.Pop();
    }
}