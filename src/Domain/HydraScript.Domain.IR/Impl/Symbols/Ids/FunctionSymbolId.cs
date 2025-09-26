using Cysharp.Text;

namespace HydraScript.Domain.IR.Impl.Symbols.Ids;

public class FunctionSymbolId(
    string name,
    IEnumerable<Type> parameters) : SymbolId<FunctionSymbol>
{
    protected override string Value { get; } =
        $"function {name}({ZString.Join(", ", parameters)})";

    public bool HasName(string askedName) => name == askedName;
}