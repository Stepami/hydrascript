namespace HydraScript.Domain.IR.Impl.SymbolIds;

public class FunctionSymbolId(
    string id,
    IEnumerable<Type> parameters) : ISymbolId
{
    public string Value { get; } =
        $"function {id}({string.Join(", ", parameters)})";
}