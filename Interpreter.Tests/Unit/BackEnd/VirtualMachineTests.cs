#nullable enable
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Tests.Helpers;
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

        var vm = new VirtualMachine(new(), new Stack<Frame>(new[] { new Frame(new HashedAddress(0)) }), new(), writer.Object);
        var print = new Print(new Constant(223, "223"));

        print.Execute(vm);
        writer.Verify(x => x.WriteLine(
            It.Is<object?>(v => v!.Equals(223))
        ), Times.Once());
    }

    [Fact]
    public void ProgramWithoutHaltWillNotRunTest()
    {
        var program = new AddressedInstructions();
        Assert.Throws<ArgumentOutOfRangeException>(() => _vm.Run(program));
        
        program.Add(new Halt());
        Assert.Null(Record.Exception(() => _vm.Run(program)));
    }

    [Fact]
    public void VirtualMachineFramesClearedAfterExecutionTest()
    {
        var program = new List<Instruction>
        {
            new Simple("a", (new Constant(1, "1"), new Constant(2, "2")), "+"),
            new AsString(new Name("a"))
            {
                Left = "s"
            },
            new Halt()
        }.ToAddressedInstructions();
        
        _vm.Run(program);
        Assert.Empty(_vm.Frames);
    }

    [Fact]
    public void VirtualMachineHandlesRecursionTest()
    {
        var halt = new Mock<Halt>(12).Trackable();
        var factorial = new FunctionInfo("fact");
        var program = new List<Instruction>
        {
            new Goto(new Label("10")),
            new BeginFunction(factorial),
            new Simple("_t2", (new Name("n"), new Constant(2, "2")), "<"),
            new IfNotGoto(new Name("_t2"), new Label("5")),
            new Return(new Name("n")),
            new Simple("_t5", (new Name("n"), new Constant(1, "1")), "-") { Address = new Label("5") },
            new PushParameter("n", new Name("_t5")),
            new CallFunction(factorial, 1, "f"),
            new Simple("_t8", (new Name("n"), new Name("f")), "*"),
            new Return(new Name("_t8")),
            new PushParameter("n", new Constant(6, "6")) { Address = new Label("10") },
            new CallFunction(factorial, 1,  "fa6"),
            halt.Object
        }.ToAddressedInstructions();
        
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
        vm.Frames.Push(new Frame(new HashedAddress(0)));
            
        var createArray = new CreateArray("arr", 6);
        createArray.Execute(vm);
        Assert.Equal(6, ((List<object>) vm.Frames.Peek()["arr"]).Count);

        var indexAssignment = new IndexAssignment("arr", (new Constant(0, "0"), new Constant(0, "0")));
        indexAssignment.Execute(vm);
        Assert.Equal(0, ((List<object>) vm.Frames.Peek()["arr"])[0]);

        var removeFromArray = new RemoveFromArray("arr", new Constant(5, "5"));
        removeFromArray.Execute(vm);
        Assert.Equal(5, ((List<object>) vm.Frames.Peek()["arr"]).Count);
    }

    [Fact]
    public void ObjectCreationTest()
    {
        var halt = new Mock<Halt>(2).Trackable();
        var program = new List<Instruction>
        {
            new CreateObject("obj"),
            new DotAssignment("obj", (new Constant("prop", "prop"), new Constant(null, "null"))),
            halt.Object
        }.ToAddressedInstructions();
            
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