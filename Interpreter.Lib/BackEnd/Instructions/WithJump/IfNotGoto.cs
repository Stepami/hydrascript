using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithJump;

public class IfNotGoto : Goto
{
    private readonly IValue _test;

    public IfNotGoto(IValue test, Label jump) :
        base(jump) =>
        _test = test;

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        return !Convert.ToBoolean(_test.Get(frame))
            ? jump
            : Address.Next;
    }

    protected override string ToStringInternal() =>
        $"IfNot {_test} Goto {jump.Name}";
}