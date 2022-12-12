namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class VariableSymbol : Symbol
{
    public bool ReadOnly { get; }

    public VariableSymbol(string id, Type type, bool readOnly = false) :
        base(id, type) =>
        ReadOnly = readOnly;

    public override string ToString() => $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
}