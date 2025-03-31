namespace HydraScript.Domain.IR.Impl.Symbols;

public class VariableSymbol(
    string name,
    Type type,
    bool readOnly = false) : Symbol(name, type)
{
    public bool ReadOnly { get; } = readOnly;
    public bool Initialized { get; private set; } = readOnly;

    public void Initialize() => Initialized = true;

    public override string ToString() =>
        $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}