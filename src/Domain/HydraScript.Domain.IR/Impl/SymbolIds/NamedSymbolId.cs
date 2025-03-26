namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class NamedSymbolId(string name) : ISymbolId
{
    public string Value { get; } = name;
}