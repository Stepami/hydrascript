using System.Diagnostics.CodeAnalysis;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class VariableSymbol(
    string name,
    Type type,
    bool readOnly = false) : Symbol(name, type)
{
    public override VariableSymbolId Id { get; } = new(name);

    public bool ReadOnly { get; } = readOnly;
    public bool Initialized { get; private set; } = readOnly;

    public void Initialize() => Initialized = true;

    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        $"{(ReadOnly ? "const " : "")}{Name}: {Type}";
}