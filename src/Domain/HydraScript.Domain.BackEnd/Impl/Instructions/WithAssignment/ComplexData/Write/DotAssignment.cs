using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;

public class DotAssignment(string @object, IValue property, IValue value)
    : Simple(left: @object, (property, value), "."), IWriteToComplexData
{
    public override IAddress Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        if (frame[Left!] is Dictionary<string, object> obj)
        {
            var field = (string?)Right.left?.Get(frame) ?? string.Empty;
            obj[field] = Right.right!.Get(frame)!;
        }

        return Address.Next;
    }
    
    public Simple ToSimple() =>
        new DotRead(new Name(Left!), Right.left!);

    protected override string ToStringInternal() =>
        $"{Left}.{Right.left} = {Right.right}";
}