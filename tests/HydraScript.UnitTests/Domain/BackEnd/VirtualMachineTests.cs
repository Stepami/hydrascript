using AutoFixture.Xunit3;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class VirtualMachineTests
{
    [Theory, AutoHydraScriptData]
    public void CorrectPrintToOutTest([Frozen] IOutputWriter writer, IExecuteParams exParams)
    {
        exParams.CallStack.Returns(new Stack<Call>());
        exParams.Frames.Returns(new Stack<Frame>([new Frame()]));
        exParams.Arguments.Returns(new Queue<object?>());

        var print = new Print(new Constant(223))
        {
            Address = new HashAddress(1)
        };

        print.Execute(exParams);
        writer.Received(1).WriteLine(223);
    }

    [Theory, AutoHydraScriptData]
    public void Run_EmptyProgram_Success(VirtualMachine vm)
    {
        Assert.Null(Record.Exception(() => vm.Run([])));
    }

    [Theory, AutoHydraScriptData]
    public void VirtualMachineFramesClearedAfterExecutionTest(VirtualMachine vm)
    {
        AddressedInstructions program =
        [
            new Simple(new Name("a"), (new Constant(1), new Constant(2)), "+"),
            new AsString(new Name("a"))
            {
                Left = new Name("s")
            },
            new Halt()
        ];

        vm.Run(program);
        Assert.Empty(vm.ExecuteParams.Frames);
    }

    [Theory, AutoHydraScriptData]
    public void VirtualMachineHandlesRecursionTest(VirtualMachine vm)
    {
        var halt = HaltTrackable();
        var factorial = new FunctionInfo("fact");
        var program = new AddressedInstructions
        {
            new Goto(factorial.End),
            { new BeginBlock(BlockType.Function, blockId: factorial.ToString()), factorial.Start.Name },
            new PopParameter(new Name("n"), defaultValue: null),
            new Simple(new Name("_t2"), (new Name("n"), new Constant(2)), "<"),
            new IfNotGoto(new Name("_t2"), new Label("5")),
            new Return(new Name("n")),
            { new Simple(new Name("_t5"), (new Name("n"), new Constant(1)), "-"), "5" },
            new PushParameter(new Name("_t5")),
            new CallFunction(factorial, true)
            {
                Left = new Name("f")
            },
            new Simple(new Name("_t8"), (new Name("n"), new Name("f")), "*"),
            new Return(new Name("_t8")),
            { new EndBlock(BlockType.Function, blockId: factorial.ToString()), factorial.End.Name },
            new PushParameter(new Constant(6)),
            new CallFunction(factorial, true)
            {
                Left = new Name("fa6")
            },
            halt
        };

        vm.Run(program);
        Assert.Empty(vm.ExecuteParams.CallStack);
        Assert.Empty(vm.ExecuteParams.Arguments);
        halt.Received(1).Execute(
            Arg.Is<IExecuteParams>(
                vmParam =>
                    Convert.ToInt32(vmParam.Frames.Peek()["fa6"]) == 720));
        vm.ExecuteParams.Frames.Pop();
    }

    [Theory, AutoHydraScriptData]
    public void CreateArrayReservesCertainSpaceTest(ExecuteParams vm)
    {
        vm.Frames.Push(new Frame());

        var createArray = new CreateArray(new Name("arr"), 6)
        {
            Address = new HashAddress(1)
        };
        createArray.Execute(vm);
        Assert.Equal(6, ((List<object>)vm.Frames.Peek()["arr"]!).Count);

        var indexAssignment = new IndexAssignment(new Name("arr"), new Constant(0), new Constant(0))
        {
            Address = new HashAddress(2)
        };
        indexAssignment.Execute(vm);
        Assert.Equal(0, ((List<object>)vm.Frames.Peek()["arr"]!)[0]);

        var removeFromArray = new RemoveFromArray(new Name("arr"), new Constant(5))
        {
            Address = new HashAddress(3)
        };
        removeFromArray.Execute(vm);
        Assert.Equal(5, ((List<object>)vm.Frames.Peek()["arr"]!).Count);
    }

    [Theory, AutoHydraScriptData]
    public void ObjectCreationTest(VirtualMachine vm)
    {
        var halt = HaltTrackable();
        AddressedInstructions program =
        [
            new CreateObject(new Name("obj")),
            new DotAssignment(new Name("obj"), new Constant("prop"), new Constant(null, "null")),
            halt
        ];

        vm.Run(program);
        halt.Received(1).Execute(
            Arg.Is<IExecuteParams>(
                vmParam =>
                    ((Dictionary<string, object?>)vmParam.Frames.Peek()["obj"]!)["prop"] == null));
        vm.ExecuteParams.Frames.Pop();
    }

    private static Halt HaltTrackable()
    {
        IAddress? empty = null;
        var halt = Substitute.For<Halt>();
        halt.Execute(null!).ReturnsForAnyArgs(empty);
        return halt;
    }
}