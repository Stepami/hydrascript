namespace HydraScript.Domain.IR.Impl.Symbols.Ids;

public class VariableSymbolId(string name) : SymbolId<VariableSymbol>
{
    protected override string Value { get; } = "var " + name;
}