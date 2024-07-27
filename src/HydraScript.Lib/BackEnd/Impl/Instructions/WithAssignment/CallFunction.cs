namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment;

public class CallFunction(
    FunctionInfo function,
    int numberOfArguments,
    bool hasReturnValue) : Simple(null, (null, null), "Call ")
{
    protected override void OnSetOfAddress(IAddress address)
    {
        if (hasReturnValue)
            base.OnSetOfAddress(address);
    }

    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = new Frame(Address.Next, executeParams.Frames.Peek());

        var i = 0;
        var args = new List<CallArgument>();
        while (i < numberOfArguments)
        {
            args.Add(executeParams.Arguments.Pop());
            frame[args[i].Id] = args[i].Value;
            i++;
        }

        executeParams.CallStack.Push(new Call(Address, function, args, Left));
        executeParams.Frames.Push(frame);
        return function.Start;
    }

    protected override string ToStringInternal() => Left == null
        ? $"Call {function}, {numberOfArguments}"
        : $"{Left} = Call {function}, {numberOfArguments}";
}