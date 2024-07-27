using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Services;

public interface IMethodStorage
{
    void BindMethod(ObjectType objectType, FunctionSymbol method);

    IReadOnlyDictionary<string, FunctionSymbol> GetAvailableMethods(ObjectType objectType);
}