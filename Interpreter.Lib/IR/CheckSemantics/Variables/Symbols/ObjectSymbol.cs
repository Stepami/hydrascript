using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class ObjectSymbol : VariableSymbol
{
    public override ObjectType Type { get; }

    public SymbolTable Table { get; init; }
        
    public ObjectSymbol(string id, ObjectType objectType, bool readOnly = false, SymbolTable table = null) :
        base(id, objectType, readOnly)
    {
        Type = objectType;
        if (table != null)
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
}