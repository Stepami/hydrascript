using HydraScript.Domain.IR.Impl.SymbolIds;

namespace HydraScript.Domain.IR.Impl.Symbols;

public abstract class Symbol(string id, Type type) : ISymbol
{
    public virtual ISymbolId Id { get; } = new NamedSymbolId(id);
    public virtual Type Type { get; } = type;
}