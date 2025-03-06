using System.Collections;
using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class InstructionsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return
        [
            new AsString(new Name("num"))
            {
                Left = "str"
            },
            "str = num as string"
        ];
        yield return
        [
            new BeginBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("Start_func")
            },
            "Start_func:\n\tBeginFunction func"
        ];
        yield return
        [
            new CallFunction(new FunctionInfo("func"), false),
            "Call func"
        ];
        yield return
        [
            new CallFunction(new FunctionInfo("func"), true)
            {
                Left = "ret"
            },
            "ret = Call func"
        ];
        yield return
        [
            new CreateArray("arr", 5),
            "array arr = [5]"
        ];
        yield return
        [
            new CreateObject("obj"),
            "object obj = {}"
        ];
        yield return
        [
            new DotAssignment("obj", new Constant("prop"), new Constant(3)),
            "obj.prop = 3"
        ];
        yield return
        [
            new EndBlock(BlockType.Function, blockId: "func")
            {
                Address = new Label("End_func")
            },
            "End_func:\n\tEndFunction func"
        ];
        yield return
        [
            new Goto(new Label("10")),
            "Goto 10"
        ];
        yield return
        [
            new Halt(),
            "End"
        ];
        yield return
        [
            new IfNotGoto(new Name("test"), new Label("17")),
            "IfNot test Goto 17"
        ];
        yield return
        [
            new IndexAssignment("arr", new Constant(1), new Constant(1)),
            "arr[1] = 1"
        ];
        yield return
        [
            new Print(new Name("str")),
            "Print str"
        ];
        yield return
        [
            new PushParameter(new Name("value")),
            "PushParameter value"
        ];
        yield return
        [
            new PopParameter("param"),
            "PopParameter param"
        ];
        yield return
        [
            new RemoveFromArray("arr", new Constant(0)),
            "RemoveFrom arr at 0"
        ];
        yield return
        [
            new Return(),
            "Return"
        ];
        yield return
        [
            new Return(new Name("result")),
            "Return result"
        ];
        yield return
        [
            new Simple("a", (new Name("b"), new Name("c")), "+"),
            "a = b + c"
        ];
        yield return
        [
            new Simple("b", (null, new Name("c")), "-"),
            "b = -c"
        ];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}