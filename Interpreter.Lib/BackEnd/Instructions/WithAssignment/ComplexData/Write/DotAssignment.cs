using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Read;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Write;

public class DotAssignment : Simple, IWriteToComplexData
{
    public DotAssignment(string @object, IValue property, IValue value) :
        base(left: @object, (property, value), ".") { }

    public override IAddress Execute(VirtualMachine vm)
    {
        var frame = vm.Frames.Peek();
        var obj = (Dictionary<string, object>) frame[Left];
        var field = (string) right.left.Get(frame) ?? string.Empty;
        obj[field] = right.right.Get(frame);
        return Address.Next;
    }
    
    public Simple ToSimple() =>
        new DotRead(new Name(Left), right.left);

    protected override string ToStringInternal() =>
        $"{Left}.{right.left} = {right.right}";
}