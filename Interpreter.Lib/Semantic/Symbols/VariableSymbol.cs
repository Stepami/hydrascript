using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Symbols
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