namespace HydraScript.Domain.IR.Impl.Symbols;

public class VariableSymbol(
    string id,
    Type type,
    bool readOnly = false) : Symbol(id, type)
{
    public bool ReadOnly { get; } = readOnly;

    public override string ToString() =>
        $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}