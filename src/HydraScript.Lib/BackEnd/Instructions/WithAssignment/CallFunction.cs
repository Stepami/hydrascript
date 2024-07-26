namespace HydraScript.Lib.BackEnd.Instructions.WithAssignment;

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

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = new Frame(Address.Next, vm.Frames.Peek());

        var i = 0;
        var args = new List<CallArgument>();
        while (i < _numberOfArguments)
        {
            args.Add(vm.Arguments.Pop());
            frame[args[i].Id] = args[i].Value;
            i++;
        }

        vm.CallStack.Push(new Call(Address, _function, args, Left));
        vm.Frames.Push(frame);
        return _function.Start;
    }

    protected override string ToStringInternal() => Left == null
        ? $"Call {_function}, {_numberOfArguments}"
        : $"{Left} = Call {_function}, {_numberOfArguments}";
}