namespace HydraScript.Domain.IR;

public interface ISymbol
{
    public ISymbolId Id { get; }
    public Type Type { get; }
}