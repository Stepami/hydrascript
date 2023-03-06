using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions;

public class IndexAssignment : Simple
{
    public IndexAssignment(string array, IValue index, IValue value) : 
        base(left: array, right: (index, value), "[]") { }

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        var obj = (List<object>) frame[Left];
        var index = Convert.ToInt32(right.left.Get(frame));
        obj[index] = right.right.Get(frame);
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"{Left}[{right.left}] = {right.right}";
}