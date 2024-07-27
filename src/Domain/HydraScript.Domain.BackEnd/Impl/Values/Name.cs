namespace HydraScript.Domain.BackEnd.Impl.Values;

public class Name(string id) : IValue
{
    private readonly string _id = id;

    public object? Get(Frame? frame) => frame![_id];

    public override string ToString() => _id;

    public bool Equals(IValue? other) =>
        other is Name that &&
        _id == that._id;
}