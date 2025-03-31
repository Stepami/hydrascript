namespace HydraScript.Domain.IR;

public interface ISymbol
{
    public SymbolId Id { get; }
    public string Name { get; }
    public Type Type { get; }
}