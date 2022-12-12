using System.Collections;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Tests.TestData;

public class InstructionsData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new AsString("str", new Name("num"), 0),
            "0: str = num as string"
        };
        yield return new object[]
        {
            new BeginFunction(1, new FunctionInfo("func", 1)),
            "1: BeginFunction func"
        };
        yield return new object[]
        {
            new CallFunction(new FunctionInfo("func"), 2, 0),
            "2: Call (0, func), 0"
        };
        yield return new object[]
        {
            new CallFunction(new FunctionInfo("func"), 2, 0, "ret"),
            "2: ret = Call (0, func), 0"
        };
        yield return new object[]
        {
            new CreateArray(3, "arr", 5),
            "3: array arr = [5]"
        };
        yield return new object[]
        {
            new CreateObject(4, "obj"),
            "4: object obj = {}"
        };
        yield return new object[]
        {
            new DotAssignment("obj", (new Constant("prop", "prop"), new Constant(3, "3")), 5),
            "5: obj.prop = 3"
        };
        yield return new object[]
        {
            new Goto(10, 6),
            "6: Goto 10"
        };
        yield return new object[]
        {
            new Halt(7),
            "7: End"
        };
        yield return new object[]
        {
            new IfNotGoto(new Name("test"), 17, 8),
            "8: IfNot test Goto 17"
        };
        yield return new object[]
        {
            new IndexAssignment("arr", (new Constant(1, "1"), new Constant(1, "1")), 9),
            "9: arr[1] = 1"
        };
        yield return new object[]
        {
            new Print(10, new Name("str")),
            "10: Print str"
        };
        yield return new object[]
        {
            new PushParameter(11, "param", new Name("value")),
            "11: PushParameter param = value"
        };
        yield return new object[]
        {
            new RemoveFromArray(12, "arr", new Constant(0, "0")),
            "12: RemoveFrom arr at 0"
        };
        yield return new object[]
        {
            new Return(3, 13),
            "13: Return"
        };
        yield return new object[]
        {
            new Return(3, 13, new Name("result")),
            "13: Return result"
        };
        yield return new object[]
        {
            new Simple("a", (new Name("b"), new Name("c")), "+", 14),
            "14: a = b + c"
        };
        yield return new object[]
        {
            new Simple("b", (null, new Name("c")), "-", 14),
            "14: b = -c"
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}