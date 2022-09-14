using Type = Interpreter.Lib.IR.CheckSemantics.Types.Type;

namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols
{
    public class VariableSymbol : Symbol
    {
        public Type Type { get; init; }

        public bool ReadOnly { get; }

        public VariableSymbol(string id, bool readOnly = false) : base(id)
        {
            ReadOnly = readOnly;
        }

        public override string ToString() => $"{(ReadOnly ? "const " : "")}{Id}: {Type}";
    }
}