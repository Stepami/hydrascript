namespace Interpreter.Lib.BackEnd.Addresses;

public class HashAddress : IAddress
{
    private readonly int _seed;
    
    public IAddress Next { get; set; }

    public HashAddress(int seed) =>
        _seed = seed;

    public bool Equals(IAddress other)
    {
        if (other is HashAddress hashed)
            return _seed == hashed._seed;

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