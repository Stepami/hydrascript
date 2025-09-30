namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Halt : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        executeParams.Frames.Pop();
        return null;
    }

    protected override string ToStringInternal() => "End";
}