namespace Interpreter.Lib.Semantic.Symbols
{
    public class ObjectSymbol : VariableSymbol
    {
        public ObjectSymbol(string id, bool readOnly = false) : base(id, readOnly)
        {
        }

        public SymbolTable Table { get; init; }
    }
}