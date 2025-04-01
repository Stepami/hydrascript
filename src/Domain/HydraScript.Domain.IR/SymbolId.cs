namespace HydraScript.Domain.IR;

public abstract class SymbolId<TSymbol> : ISymbolId<TSymbol>
    where TSymbol : class, ISymbol
{
    protected abstract string Value { get; }

    public bool Equals(ISymbolId<ISymbol>? other) =>
        Value == other?.ToString();

    public override bool Equals(object? obj)
    {
        if (obj is ISymbolId<ISymbol> other)
            return Equals(other);
        return false;
    }

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}