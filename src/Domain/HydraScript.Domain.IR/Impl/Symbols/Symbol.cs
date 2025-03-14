namespace HydraScript.Domain.IR.Impl.Symbols;

public abstract class Symbol(string id, Type type) : ISymbol
{
    public virtual string Id { get; } = id;
    public virtual Type Type { get; } = type;
}