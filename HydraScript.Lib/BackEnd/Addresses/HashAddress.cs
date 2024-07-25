namespace HydraScript.Lib.BackEnd.Addresses;

public class HashAddress(int seed) : IAddress
{
    private readonly int _seed = seed;

    public IAddress Next { get; set; } = default!;

    public bool Equals(IAddress? other) =>
        other is HashAddress hashed &&
        _seed == hashed._seed;

    public override int GetHashCode()
    {
        var i1 = _seed ^ 17;
        var i2 = 31 * _seed + i1;

        return HashCode.Combine(i1, i2);
    }

    public override string ToString() => "\t";
}