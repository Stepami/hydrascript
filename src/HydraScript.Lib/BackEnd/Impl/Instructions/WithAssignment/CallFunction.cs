namespace HydraScript.Lib.BackEnd.Impl.Instructions.WithAssignment;

public class CallFunction : Simple
{
    private readonly FunctionInfo _function;
    private readonly int _numberOfArguments;
    private readonly bool _hasReturnValue;

    public CallFunction(
        FunctionInfo function,
        int numberOfArguments,
        bool hasReturnValue) :
        base(null, (null, null), "Call ")
    {
        _function = function;
        _numberOfArguments = numberOfArguments;
        _hasReturnValue = hasReturnValue;
    }

    protected override void OnSetOfAddress(IAddress address)
    {
        if (_hasReturnValue)
            base.OnSetOfAddress(address);
    }

    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = new Frame(Address.Next, executeParams.Frames.Peek());

        var i = 0;
        var args = new List<CallArgument>();
        while (i < _numberOfArguments)
        {
            args.Add(executeParams.Arguments.Pop());
            frame[args[i].Id] = args[i].Value;
            i++;
        }

        executeParams.CallStack.Push(new Call(Address, _function, args, Left));
        executeParams.Frames.Push(frame);
        return _function.Start;
    }

    protected override string ToStringInternal() => Left == null
        ? $"Call {_function}, {_numberOfArguments}"
        : $"{Left} = Call {_function}, {_numberOfArguments}";
}