namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class FunctionSymbolId(
    string name,
    IEnumerable<Type> parameters) : SymbolId
{
    protected override string Value { get; } =
        $"function {name}({string.Join(", ", parameters)})";

    public bool HasName(string askedName) => name == askedName;
}