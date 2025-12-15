namespace HydraScript.Domain.BackEnd.Impl.Instructions;

public class Return(IValue? value = null) : Instruction
{
    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var callFrame = executeParams.Frames.Pop();
        var call = executeParams.CallStack.Pop();
        if (call.Where != null && value != null)
        {
            var frame = executeParams.Frames.Peek();
            call.Where?.Set(frame, value.Get(callFrame));
        }

        return call.From.Next;
    }

    protected override string ToStringInternal() =>
        $"Return{(value != null ? $" {value}" : "")}";
}