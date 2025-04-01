namespace HydraScript.Domain.IR.Impl.Symbols;

public abstract class Symbol(string name, Type type) : ISymbol
{
    public abstract ISymbolId<ISymbol> Id { get; }
    public string Name { get; } = name;
    public virtual Type Type { get; } = type;
}