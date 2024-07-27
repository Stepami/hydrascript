using HydraScript.Lib.BackEnd.Impl.Addresses;

namespace HydraScript.Lib.BackEnd.Impl.Instructions;

public class Halt : Instruction
{
    public override bool End => true;

    public override IAddress Execute(IExecuteParams executeParams)
    {
        executeParams.Frames.Pop();
        return new HashAddress(seed: 0);
    }

    protected override string ToStringInternal() => "End";
}