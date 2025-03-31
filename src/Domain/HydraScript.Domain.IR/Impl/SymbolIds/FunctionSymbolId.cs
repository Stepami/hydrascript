namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class FunctionSymbolId(
    string id,
    IEnumerable<Type> parameters) : SymbolId
{
    protected override string Value { get; } =
        $"function {id}({string.Join(", ", parameters)})";
}