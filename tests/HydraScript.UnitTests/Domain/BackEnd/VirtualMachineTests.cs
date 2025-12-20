using AutoFixture.Xunit3;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class VirtualMachineTests
{
    [Theory, AutoHydraScriptData]
    public void CorrectPrintToOutTest([Frozen] IConsole console, TestVirtualMachine vm)
    {
        var print = new Output(new Constant(223))
        {
            Address = new HashAddress(1)
        };

        print.Execute(vm.ExecuteParams);
        console.Received(1).WriteLine(223);
    }

    [Theory, AutoHydraScriptData]
    public void Run_EmptyProgram_Success(TestVirtualMachine vm)
    {
        Assert.Null(Record.Exception(() => vm.Run([])));
    }

    [Theory, AutoHydraScriptData]
    public void VirtualMachineFramesClearedAfterExecutionTest(TestVirtualMachine vm)
    {
        AddressedInstructions program =
        [
            new Simple(new Name("a", vm.Frame), (new Constant(1), new Constant(2)), "+"),
            new AsString(new Name("a", vm.Frame))
            {
                Left = new Name("s", vm.Frame)
            },
            new Halt()
        ];

        vm.Run(program);
        Assert.Throws<InvalidOperationException>(() => vm.ExecuteParams.FrameContext.Current);
    }

    [Theory, AutoHydraScriptData]
    public void VirtualMachineHandlesRecursionTest(TestVirtualMachine vm)
    {
        var halt = HaltTrackable();
        var factorial = new FunctionInfo("fact");
        var program = new AddressedInstructions
        {
            new Goto(factorial.End),
            { new BeginBlock(BlockType.Function, blockId: factorial.ToString()), factorial.Start.Name },
            new PopParameter(new Name("n", vm.Frame), defaultValue: null),
            new Simple(new Name("_t2", vm.Frame), (new Name("n", vm.Frame), new Constant(2)), "<"),
            new IfNotGoto(new Name("_t2", vm.Frame), new Label("5")),
            new Return(new Name("n", vm.Frame)),
            { new Simple(new Name("_t5", vm.Frame), (new Name("n", vm.Frame), new Constant(1)), "-"), "5" },
            new PushParameter(new Name("_t5", vm.Frame)),
            new CallFunction(factorial, true)
            {
                Left = new Name("f", vm.Frame)
            },
            new Simple(new Name("_t8", vm.Frame), (new Name("n", vm.Frame), new Name("f", vm.Frame)), "*"),
            new Return(new Name("_t8", vm.Frame)),
            { new EndBlock(BlockType.Function, blockId: factorial.ToString()), factorial.End.Name },
            new PushParameter(new Constant(6)),
            new CallFunction(factorial, true)
            {
                Left = new Name("fa6", vm.Frame)
            },
            halt
        };

        vm.Run(program);
        Assert.Empty(vm.ExecuteParams.CallStack);
        Assert.Empty(vm.ExecuteParams.Arguments);
        halt.Received(1).Execute(
            Arg.Is<IExecuteParams>(
                vmParam =>
                    Convert.ToInt32(vmParam.FrameContext.Current["fa6"]) == 720));
        vm.ExecuteParams.FrameContext.StepOut();
    }

    [Theory, AutoHydraScriptData]
    public void CreateArrayReservesCertainSpaceTest(TestVirtualMachine vm)
    {
        vm.ExecuteParams.FrameContext.StepIn();

        var createArray = new CreateArray(new Name("arr", vm.Frame), 6)
        {
            Address = new HashAddress(1)
        };
        createArray.Execute(vm.ExecuteParams);
        Assert.Equal(6, ((List<object>)vm.Frame["arr"]!).Count);

        var indexAssignment = new IndexAssignment(new Name("arr", vm.Frame), new Constant(0), new Constant(0))
        {
            Address = new HashAddress(2)
        };
        indexAssignment.Execute(vm.ExecuteParams);
        Assert.Equal(0, ((List<object>)vm.Frame["arr"]!)[0]);

        var removeFromArray = new RemoveFromArray(new Name("arr", vm.Frame), new Constant(5))
        {
            Address = new HashAddress(3)
        };
        removeFromArray.Execute(vm.ExecuteParams);
        Assert.Equal(5, ((List<object>)vm.Frame["arr"]!).Count);
    }

    [Theory, AutoHydraScriptData]
    public void ObjectCreationTest(TestVirtualMachine vm)
    {
        var halt = HaltTrackable();
        AddressedInstructions program =
        [
            new CreateObject(new Name("obj", vm.Frame)),
            new DotAssignment(new Name("obj", vm.Frame), new Constant("prop"), new Constant(null, "null")),
            halt
        ];

        vm.Run(program);
        halt.Received(1).Execute(
            Arg.Is<IExecuteParams>(
                vmParam =>
                    ((Dictionary<string, object?>)vmParam.FrameContext.Current["obj"]!)["prop"] == null));
        vm.ExecuteParams.FrameContext.StepOut();
    }

    private static Halt HaltTrackable()
    {
        IAddress? empty = null;
        var halt = Substitute.For<Halt>();
        halt.Execute(null!).ReturnsForAnyArgs(empty);
        return halt;
    }
}