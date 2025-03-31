using HydraScript.Domain.IR.Types;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class ObjectSymbol(
    string name,
    ObjectType objectType,
    bool readOnly = false) : VariableSymbol(name, objectType, readOnly)
{
    public override ObjectType Type { get; } = objectType;
}