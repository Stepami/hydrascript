using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;

public class IndexAssignment(Name array, IValue index, IValue value)
    : Simple(left: array, right: (index, value), "[]"), IWriteToComplexData
{
    protected override void Assign()
    {
        if (Left?.Get() is not List<object> list)
            return;

        var index = Convert.ToInt32(Right.left!.Get());
        list[index] = Right.right!.Get()!;
    }

    public Simple ToSimple() =>
        new IndexRead(Left!, Right.left!);

    protected override string ToStringInternal() =>
        $"{Left}[{Right.left}] = {Right.right}";
}