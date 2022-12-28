using System.Collections;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class Return : Instruction, IEnumerable<IAddress>
{
    private readonly IValue _value;
    private readonly List<IAddress> _callers = new();

    public IAddress FunctionStart { get; }

    public Return(IAddress functionStart, IValue value = null) =>
        (FunctionStart, _value) = (functionStart, value);

    public void AddCaller(IAddress caller) =>
        _callers.Add(caller);

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Pop();
        var call = vm.CallStack.Pop();
        if (call.Where != null && _value != null)
        {
            vm.Frames.Peek()[call.Where] = _value.Get(frame);
        }

        return frame.ReturnAddress;
    }

    public IEnumerator<IAddress> GetEnumerator() =>
        _callers.GetEnumerator();
        
    IEnumerator IEnumerable.GetEnumerator() =>
        GetEnumerator();

    protected override string ToStringInternal() =>
        $"Return{(_value != null ? $" {_value}" : "")}";
}