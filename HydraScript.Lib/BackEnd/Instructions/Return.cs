using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.BackEnd.Instructions;

public class Return : Instruction
{
    private readonly IValue _value;

    public Return(IValue value = null) =>
        _value = value;

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

    protected override string ToStringInternal() =>
        $"Return{(_value != null ? $" {_value}" : "")}";
}