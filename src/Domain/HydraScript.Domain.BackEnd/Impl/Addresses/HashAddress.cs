namespace HydraScript.Domain.BackEnd.Impl.Addresses;

public class HashAddress(int seed) : IAddress
{
    private readonly int _seed = seed;
    private string? _name;

    private readonly Guid _id = Guid.NewGuid();

    public IAddress Next { get; set; } = default!;

    public string Name
    {
        get
        {
            if (_name is null)
            {
                var baseName = $"{unchecked((uint)GetHashCode())}{_id:N}";
                var nameArray = Random.Shared.GetItems(baseName.AsSpan(), 10);
                _name = "_t" + new string(nameArray);
            }

            return _name;
        }
    }

    public bool Equals(IAddress? other)
    {
        if (other is HashAddress hashed)
            return _seed == hashed._seed && _id == hashed._id;

        return false;
    }

    public override int GetHashCode()
    {
        var i1 = _seed ^ 17;
        var i2 = 31 * _seed + i1;

        return HashCode.Combine(i1, i2);
    }

    public override string ToString() => "\t";
}
