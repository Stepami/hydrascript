namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Return(IValue? value = null) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var call = executeParams.CallStack.Pop();
        if (call.Where != null && value != null)
        {
            var returnValue = value.Get();
            executeParams.FrameContext.StepOut();
            call.Where?.Set(returnValue);
        }
        else
        {
            executeParams.FrameContext.StepOut();
        }

        return call.From.Next;
    }

    protected override string ToStringInternal() =>
        $"Return{(value != null ? $" {value}" : "")}";
}