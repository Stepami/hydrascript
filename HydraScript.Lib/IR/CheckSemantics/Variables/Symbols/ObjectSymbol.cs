using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

public class ObjectSymbol : VariableSymbol
{
    public override ObjectType Type { get; }

    public ObjectSymbol(string id, ObjectType objectType, bool readOnly = false) :
        base(id, objectType, readOnly)
    {
        Type = objectType;
    }
}