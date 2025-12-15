namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;

public class CallFunction(
    FunctionInfo function,
    bool hasReturnValue) : Simple(null, (null, null), "Call ")
{
    protected override void OnSetOfAddress(IAddress address)
    {
        if (hasReturnValue)
            base.OnSetOfAddress(address);
    }

    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = new Frame(executeParams.Frames.Peek());
        executeParams.CallStack.Push(new Call(Address, function, Left));
        executeParams.Frames.Push(frame);
        return function.Start;
    }

    protected override string ToStringInternal() => Left == null
        ? $"Call {function}"
        : $"{Left} = Call {function}";
}