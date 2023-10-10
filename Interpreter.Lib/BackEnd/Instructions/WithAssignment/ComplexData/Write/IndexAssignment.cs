using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;

public class IndexAssignment : Simple, IWriteToComplexData
{
    public IndexAssignment(string array, IValue index, IValue value) : 
        base(left: array, right: (index, value), "[]") { }

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        var obj = (List<object>) frame[Left];
        var index = Convert.ToInt32(Right.left.Get(frame));
        obj[index] = Right.right.Get(frame);
        return Address.Next;
    }

    public Simple ToSimple() =>
        new IndexRead(new Name(Left), Right.left);

    protected override string ToStringInternal() =>
        $"{Left}[{Right.left}] = {Right.right}";
}