namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class TypeSymbolId(string name) : SymbolId
{
    protected override string Value { get; } = "type " + name;
}