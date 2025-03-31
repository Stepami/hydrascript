using HydraScript.Domain.IR.Impl.SymbolIds;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis;

public interface IMethodStorage
{
    public void BindMethod(ObjectType objectType, FunctionSymbol method);

    public IReadOnlyDictionary<FunctionSymbolId, FunctionSymbol> GetAvailableMethods(ObjectType objectType);
}