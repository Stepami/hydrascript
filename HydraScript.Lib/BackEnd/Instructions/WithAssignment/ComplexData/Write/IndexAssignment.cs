using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Lib.BackEnd.Values;

namespace HydraScript.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;

public class IndexAssignment(string array, IValue index, IValue value)
    : Simple(left: array, right: (index, value), "[]"), IWriteToComplexData
{
    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        if (frame[Left!] is List<object> list)
        {
            var index = Convert.ToInt32(Right.left!.Get(frame));
            list[index] = Right.right!.Get(frame)!;
        }

        return Address.Next;
    }

    public Simple ToSimple() =>
        new IndexRead(new Name(Left!), Right.left!);

    protected override string ToStringInternal() =>
        $"{Left}[{Right.left}] = {Right.right}";
}