namespace HydraScript.Domain.IR.Impl.Symbols;

public class VariableSymbol(
    string id,
    Type type,
    bool readOnly = false) : Symbol(id, type)
{
    private bool _initialized = readOnly;

    public bool ReadOnly { get; } = readOnly;
    public override bool Initialized => _initialized;

    public void Initialize() => _initialized = true;

    public override string ToString() =>
        $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}