using HydraScript.Domain.BackEnd.Impl.Addresses;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;

public class IfNotGoto(IValue test, Label jump) : Goto(jump)
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        return !Convert.ToBoolean(test.Get())
            ? Jump
            : Address.Next;
    }

    protected override string ToStringInternal() =>
        $"IfNot {test} Goto {Jump.Name}";
}