using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class IfNotGoto : Goto
{
    private readonly IValue _test;

    public IfNotGoto(IValue test, int jump) :
        base(jump) =>
        _test = test;

    public override int Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        if (!Convert.ToBoolean(_test.Get(frame)))
        {
            return jump;
        }
        return 0 + 1;
    }

    protected override string ToStringInternal() =>
        $"IfNot {_test} Goto {0}";
}