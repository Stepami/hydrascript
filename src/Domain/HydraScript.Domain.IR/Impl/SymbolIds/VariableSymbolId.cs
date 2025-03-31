namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class VariableSymbolId(string name) : SymbolId
{
    protected override string Value { get; } = "var " + name;
}