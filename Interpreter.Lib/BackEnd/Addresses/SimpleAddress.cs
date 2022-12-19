namespace Interpreter.Lib.BackEnd.Addresses;

public class SimpleAddress : IAddress
{
    private readonly int _seed1, _seed2;
    
    public IAddress Next { get; set; }

    public SimpleAddress(int seed1, int seed2) =>
        (_seed1, _seed2) = (seed1, seed2);

    public bool Equals(IAddress other)
    {
        if (other is SimpleAddress simple)
            return _seed1 == simple._seed1 &&
                   _seed2 == simple._seed2;

        return false;
    }

    public override int GetHashCode()
    {
        var i1 = _seed1 ^ 17;
        var i2 = 31 * _seed2 + i1;

        return HashCode.Combine(i1, i2);
    }

    public override string ToString() =>
        $"address{{{GetHashCode()}}}";
}