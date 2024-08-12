namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Return(IValue? value = null) : Instruction
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Pop();
        var call = executeParams.CallStack.Pop();
        if (call.Where != null && value != null)
        {
            executeParams.Frames.Peek()[call.Where] = value.Get(frame);
        }

        return frame.ReturnAddress;
    }

    protected override string ToStringInternal() =>
        $"Return{(value != null ? $" {value}" : "")}";
}