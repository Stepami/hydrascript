namespace HydraScript.Domain.IR;

public abstract class SymbolId : IEquatable<SymbolId>
{
    protected abstract string Value { get; }

    public bool Equals(SymbolId? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((SymbolId)obj);
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}