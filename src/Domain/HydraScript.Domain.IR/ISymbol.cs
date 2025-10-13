namespace HydraScript.Domain.IR;

public interface ISymbol
{
    public ISymbolId<ISymbol> Id { get; }
    public Type Type { get; }
}