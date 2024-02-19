#nullable enable
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Create;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;
using Interpreter.Lib.BackEnd.Instructions.WithJump;
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

        var vm = new VirtualMachine(new(), new Stack<Frame>(new[] { new Frame(new HashAddress(0)) }), new(), writer.Object);
        var print = new Print(new Constant(223))
        {
            Address = new HashAddress(1)
        };

        print.Execute(vm);
        writer.Verify(x => x.WriteLine(
            It.Is<object?>(v => v!.Equals(223))
        ), Times.Once());
    }

    [Fact]
    public void ProgramWithoutHaltWillNotRunTest()
    {
        var program = new AddressedInstructions();
        Assert.Throws<ArgumentNullException>(() => _vm.Run(program));
        
        program.Add(new Halt());
        Assert.Null(Record.Exception(() => _vm.Run(program)));
    }

    [Fact]
    public void VirtualMachineFramesClearedAfterExecutionTest()
    {
        var program = new List<Instruction>
        {
            new Simple("a", (new Constant(1), new Constant(2)), "+"),
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
        var halt = new Mock<Halt>().Trackable();
        var factorial = new FunctionInfo("fact");
        var program = new AddressedInstructions
        {
            new Goto(factorial.End),
            { new BeginBlock(BlockType.Function, blockId: factorial.ToString()), factorial.Start.Name },
            new Simple("_t2", (new Name("n"), new Constant(2)), "<"),
            new IfNotGoto(new Name("_t2"), new Label("5")),
            new Return(new Name("n")),
            { new Simple("_t5", (new Name("n"), new Constant(1)), "-"), "5" },
            new PushParameter("n", new Name("_t5")),
            new CallFunction(factorial, 1, true)
            {
                Left = "f"
            },
            new Simple("_t8", (new Name("n"), new Name("f")), "*"),
            new Return(new Name("_t8")),
            { new EndBlock(BlockType.Function, blockId: factorial.ToString()), factorial.End.Name },
            new PushParameter("n", new Constant(6)),
            new CallFunction(factorial, 1, true)
            {
                Left = "fa6"
            },
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
        vm.Frames.Push(new Frame(new HashAddress(0)));
            
        var createArray = new CreateArray("arr", 6)
        {
            Address = new HashAddress(1)
        };
        createArray.Execute(vm);
        Assert.Equal(6, ((List<object>) vm.Frames.Peek()["arr"]).Count);

        var indexAssignment = new IndexAssignment("arr", new Constant(0), new Constant(0))
        {
            Address = new HashAddress(2)
        };
        indexAssignment.Execute(vm);
        Assert.Equal(0, ((List<object>) vm.Frames.Peek()["arr"])[0]);

        var removeFromArray = new RemoveFromArray("arr", new Constant(5))
        {
            Address = new HashAddress(3)
        };
        removeFromArray.Execute(vm);
        Assert.Equal(5, ((List<object>) vm.Frames.Peek()["arr"]).Count);
    }

    [Fact]
    public void ObjectCreationTest()
    {
        var halt = new Mock<Halt>().Trackable();
        var program = new List<Instruction>
        {
            new CreateObject("obj"),
            new DotAssignment("obj", new Constant("prop"), new Constant(null, "null")),
            halt.Object
        }.ToAddressedInstructions();
            
        _vm.Run(program);
        halt.Verify(x => x.Execute(
            It.Is<VirtualMachine>(
                vm => ((Dictionary<string, object?>)vm.Frames.Peek()["obj"])["prop"] == null
            )
        ), Times.Once());
        _vm.Frames.Pop();
    }
}