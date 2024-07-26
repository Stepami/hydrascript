using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Variables.Impl.Symbols;

public class ObjectSymbol(
    string id,
    ObjectType objectType,
    bool readOnly = false) : VariableSymbol(id, objectType, readOnly)
{
    public override ObjectType Type { get; } = objectType;
}