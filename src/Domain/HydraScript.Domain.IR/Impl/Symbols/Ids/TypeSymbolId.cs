namespace HydraScript.Domain.IR.Impl.Symbols.Ids;

public class TypeSymbolId(string name) : SymbolId<TypeSymbol>
{
    protected override string Value { get; } = $"type {name}";
}