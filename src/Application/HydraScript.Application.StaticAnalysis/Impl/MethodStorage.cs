using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class MethodStorage : IMethodStorage
{
    private readonly Dictionary<ObjectType, Dictionary<FunctionSymbolId, FunctionSymbol>> _bindings = [];

    public void BindMethod(ObjectType objectType, FunctionSymbol method)
    {
        objectType.AddMethod(method.Id);
        if (!_bindings.ContainsKey(objectType))
            _bindings[objectType] = new Dictionary<FunctionSymbolId, FunctionSymbol>();
        _bindings[objectType][method.Id] = method;
    }

    public IReadOnlyDictionary<FunctionSymbolId, FunctionSymbol> GetAvailableMethods(ObjectType objectType) =>
        _bindings.GetValueOrDefault(objectType, new Dictionary<FunctionSymbolId, FunctionSymbol>());
}