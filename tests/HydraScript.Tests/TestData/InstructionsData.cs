using System.Collections;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Tests.TestData;

public class InstructionsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new AsString(new Name("num"))
            {
                Left = "str"
            },
            "str = num as string"
        };
        yield return new object[]
        {
            new BeginBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("Start_func")
            },
            "Start_func:\n\tBeginFunction func"
        };
        yield return new object[]
        {
            new CallFunction(new FunctionInfo("func"), false),
            "Call func"
        };
        yield return new object[]
        {
            new CallFunction(new FunctionInfo("func"), true)
            {
                Left = "ret"
            },
            "ret = Call func"
        };
        yield return new object[]
        {
            new CreateArray("arr", 5),
            "array arr = [5]"
        };
        yield return new object[]
        {
            new CreateObject("obj"),
            "object obj = {}"
        };
        yield return new object[]
        {
            new DotAssignment("obj", new Constant("prop"), new Constant(3)),
            "obj.prop = 3"
        };
        yield return new object[]
        {
            new EndBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("End_func")
            },
            "End_func:\n\tEndFunction func"
        };
        yield return new object[]
        {
            new Goto(new Label("10")),
            "Goto 10"
        };
        yield return new object[]
        {
            new Halt(),
            "End"
        };
        yield return new object[]
        {
            new IfNotGoto(new Name("test"), new Label("17")),
            "IfNot test Goto 17"
        };
        yield return new object[]
        {
            new IndexAssignment("arr", new Constant(1), new Constant(1)),
            "arr[1] = 1"
        };
        yield return new object[]
        {
            new Print(new Name("str")),
            "Print str"
        };
        yield return new object[]
        {
            new PushParameter(new Name("value")),
            "PushParameter value"
        };
        yield return new object[]
        {
            new PopParameter("param"),
            "PopParameter param"
        };
        yield return new object[]
        {
            new RemoveFromArray("arr", new Constant(0)),
            "RemoveFrom arr at 0"
        };
        yield return new object[]
        {
            new Return(),
            "Return"
        };
        yield return new object[]
        {
            new Return(new Name("result")),
            "Return result"
        };
        yield return new object[]
        {
            new Simple("a", (new Name("b"), new Name("c")), "+"),
            "a = b + c"
        };
        yield return new object[]
        {
            new Simple("b", (null, new Name("c")), "-"),
            "b = -c"
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}