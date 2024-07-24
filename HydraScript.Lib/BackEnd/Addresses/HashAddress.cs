namespace HydraScript.Lib.BackEnd.Addresses;

public class HashAddress : IAddress
{
    private readonly int _seed;

    public IAddress Next { get; set; }

    public HashAddress(int seed) =>
        _seed = seed;

    public bool Equals(IAddress other)
    {
        if (other is HashAddress _)
            return Equals(other);

        return false;
    }

    public override bool Equals(object obj)
    {
        if (!ReferenceEquals(this, obj))
            return false;
        
        if (obj.GetType() != GetType())
            return false;

        return Equals((HashAddress)obj);
    }

    protected bool Equals(HashAddress other)
    {
        return _seed == other._seed;
    }

    public override int GetHashCode()
    {
        var i1 = _seed ^ 17;
        var i2 = 31 * _seed + i1;

        return HashCode.Combine(i1, i2);
    }

    public override string ToString() => "\t";
}