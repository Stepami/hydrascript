using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;

public class DotAssignment(Name @object, Constant property, IValue value)
    : Simple(left: @object, (property, value), "."), IWriteToComplexData
{
    protected override void Assign()
    {
        if (Left?.Get() is not Dictionary<string, object> obj)
            return;

        var field = (string?)Right.left?.Get() ?? string.Empty;
        obj[field] = Right.right!.Get()!;
    }

    public Simple ToSimple() =>
        new DotRead(Left!, property);

    protected override string ToStringInternal() =>
        $"{Left}.{Right.left} = {Right.right}";
}