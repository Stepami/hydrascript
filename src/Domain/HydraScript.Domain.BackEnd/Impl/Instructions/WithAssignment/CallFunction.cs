using HydraScript.Domain.BackEnd.Impl.Frames;

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
        Left?.SetFrame(new CurrentFrame(executeParams.FrameContext));
        executeParams.FrameContext.StepIn();
        executeParams.CallStack.Push(new Call(Address, function, Left));
        return function.Start;
    }

    protected override string ToStringInternal() => Left == null
        ? $"Call {function}"
        : $"{Left} = Call {function}";
}