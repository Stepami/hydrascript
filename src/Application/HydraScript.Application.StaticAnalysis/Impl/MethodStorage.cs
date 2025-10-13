using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class MethodStorage : IMethodStorage
{
    private readonly Dictionary<ObjectType, Dictionary<FunctionSymbolId, FunctionSymbol>> _bindings = [];

    public void BindMethod(ObjectType objectType, FunctionSymbol method, FunctionSymbolId overload)
    {
        objectType.AddMethod(overload);
        if (!_bindings.ContainsKey(objectType))
            _bindings[objectType] = new Dictionary<FunctionSymbolId, FunctionSymbol>();
        _bindings[objectType][overload] = method;
    }

    public IReadOnlyDictionary<FunctionSymbolId, FunctionSymbol> GetAvailableMethods(ObjectType objectType) =>
        _bindings.GetValueOrDefault(objectType, new Dictionary<FunctionSymbolId, FunctionSymbol>());

    public void Clear()
    {
        foreach (var objectType in _bindings.Keys)
        {
            _bindings[objectType].Clear();
        }
        _bindings.Clear();
    }
}