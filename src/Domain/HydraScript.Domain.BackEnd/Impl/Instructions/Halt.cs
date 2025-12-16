namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Halt : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        executeParams.FrameContext.StepOut();
        return null;
    }

    protected override string ToStringInternal() => "End";
}