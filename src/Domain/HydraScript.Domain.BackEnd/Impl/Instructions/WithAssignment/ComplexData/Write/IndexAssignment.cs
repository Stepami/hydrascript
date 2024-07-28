using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;

public class IndexAssignment(string array, IValue index, IValue value)
    : Simple(left: array, right: (index, value), "[]"), IWriteToComplexData
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
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