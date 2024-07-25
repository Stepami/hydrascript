using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

public interface IMethodStorage
{
    void BindMethod(ObjectType objectType, FunctionSymbol method);

    IReadOnlyDictionary<string, FunctionSymbol> GetAvailableMethods(ObjectType objectType);
}