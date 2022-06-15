using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Symbols
{
    public class ObjectSymbol : VariableSymbol
    {
        public ObjectSymbol(string id, bool readOnly = false, SymbolTable table = null, Type type = null) : base(id, readOnly)
        {
            if (table != null && type is ObjectType objectType)
            {
                foreach (var key in objectType.Keys)
                {
                    if (objectType[key] is FunctionType)
                    {
                        var function = table.FindSymbol<FunctionSymbol>(key);
                        function.CallInfo.MethodOf = id;
                    }
                }
            }
        }

        public SymbolTable Table { get; init; }
    }
}