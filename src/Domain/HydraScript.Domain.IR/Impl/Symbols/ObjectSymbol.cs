using HydraScript.Domain.IR.Types;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class ObjectSymbol(
    string id,
    ObjectType objectType,
    bool readOnly = false) : VariableSymbol(id, objectType, readOnly)
{
    public override ObjectType Type { get; } = objectType;
}