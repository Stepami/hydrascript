using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.BackEnd.Instructions;

public class Return(IValue? value = null) : Instruction
{
    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Pop();
        var call = vm.CallStack.Pop();
        if (call.Where != null && value != null)
        {
            vm.Frames.Peek()[call.Where] = value.Get(frame);
        }

        return frame.ReturnAddress;
    }

    protected override string ToStringInternal() =>
        $"Return{(value != null ? $" {value}" : "")}";
}