using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateArray(Name id, int size) : Simple(id)
{
    private readonly Name _id = id;

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        var list = new List<object>(size * 2);
        for (var i = 0; i < size; i++)
            list.Add(null!);
        _id.Set(frame, list);
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"array {_id} = [{size}]";
}