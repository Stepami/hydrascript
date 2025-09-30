namespace HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;

public class CreateObject(string id) : Simple(id)
{
    private readonly string _id = id;

    public override IAddress? Execute(IExecuteParams executeParams)
    {
        var frame = executeParams.Frames.Peek();
        frame[_id] = new Dictionary<string, object>();
        return Address.Next;
    }

    protected override string ToStringInternal() =>
        $"object {_id} = {{}}";
}