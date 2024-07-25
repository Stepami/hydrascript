namespace HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

public class VariableSymbol(string id, Type type, bool readOnly = false) : Symbol
{
    public override string Id { get; } = id;
    public override Type Type { get; } = type;
    public bool ReadOnly { get; } = readOnly;

    public override string ToString() => $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}