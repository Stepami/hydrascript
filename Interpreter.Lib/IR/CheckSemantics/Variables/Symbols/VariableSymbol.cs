namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class VariableSymbol : Symbol
{
    public override string Id { get; }
    public override Type Type { get; }
    public bool ReadOnly { get; }

    public VariableSymbol(string id, Type type, bool readOnly = false)
    {
        Id = id;
        Type = type;
        ReadOnly = readOnly;
    }

    public override string ToString() => $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}