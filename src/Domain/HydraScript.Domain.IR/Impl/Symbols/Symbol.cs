namespace HydraScript.Domain.IR.Impl.Symbols;

public abstract class Symbol(string name, Type type) : ISymbol
{
    public virtual string Id { get; } = name;
    public virtual Type Type { get; } = type;
}