namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateArray(string id, int size) : Simple(id)
{
    private readonly string _id = id;

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        var list = new List<object>(size * 2);
        for (var i = 0; i < size; i++)
            list.Add(null!);
        frame[_id] = list;
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"array {_id} = [{size}]";
}