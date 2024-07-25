using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

internal class MethodStorage : IMethodStorage
{
    private readonly Dictionary<ObjectType, Dictionary<string, FunctionSymbol>> _bindings = new();

    public void BindMethod(ObjectType objectType, FunctionSymbol method)
    {
        objectType.AddMethod(method.Id);
        if (!_bindings.ContainsKey(objectType))
            _bindings[objectType] = new Dictionary<string, FunctionSymbol>();
        _bindings[objectType][method.Id] = method;
    }

    public IReadOnlyDictionary<string, FunctionSymbol> GetAvailableMethods(ObjectType objectType) =>
        _bindings.GetValueOrDefault(objectType, new Dictionary<string, FunctionSymbol>());
}