using HydraScript.Lib.BackEnd.Addresses;

namespace HydraScript.Lib.BackEnd.Instructions.WithJump;

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
            ? Jump
            : Address.Next;
    }

    protected override string ToStringInternal() =>
        $"IfNot {_test} Goto {Jump.Name}";
}