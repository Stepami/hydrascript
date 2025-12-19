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

public class InstructionsData : TheoryData<IExecutableInstruction, string>
{
    public InstructionsData()
    {
        Add(
            new AsString(Name("num"))
            {
                Left = Name("str")
            },
            "str = num as String");
        Add(
            new BeginBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("Start_func")
            },
            "Start_func:\n\tBeginFunction func");
        Add(
            new CallFunction(new FunctionInfo("func"), false),
            "Call func");
        Add(
            new CallFunction(new FunctionInfo("func"), true)
            {
                Left = Name("ret")
            },
            "ret = Call func");
        Add(
            new CreateArray(Name("arr"), 5),
            "array arr = [5]");
        Add(
            new CreateObject(Name("obj")),
            "object obj = {}");
        Add(
            new DotAssignment(Name("obj"), new Constant("prop"), new Constant(3)),
            "obj.prop = 3");
        Add(
            new EndBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("End_func")
            },
            "End_func:\n\tEndFunction func");
        Add(
            new Goto(new Label("10")),
            "Goto 10");
        Add(
            new Halt(),
            "End");
        Add(
            new IfNotGoto(Name("test"), new Label("17")),
            "IfNot test Goto 17");
        Add(
            new IndexAssignment(Name("arr"), new Constant(1), new Constant(1)),
            "arr[1] = 1");
        Add(
            new Print(Name("str")),
            "Print str");
        Add(
            new PushParameter(Name("value")),
            "PushParameter value");
        Add(
            new PopParameter(Name("param"), defaultValue: null),
            "PopParameter param");
        Add(
            new RemoveFromArray(Name("arr"), new Constant(0)),
            "RemoveFrom arr at 0");
        Add(
            new Return(),
            "Return");
        Add(
            new Return(Name("result")),
            "Return result");
        Add(
            new Simple(Name("a"), (Name("b"), Name("c")), "+"),
            "a = b + c");
        Add(
            new Simple(Name("b"), (null, Name("c")), "-"),
            "b = -c");
    }

    private static Name Name(string id) => new(id, Substitute.For<IFrame>());
}