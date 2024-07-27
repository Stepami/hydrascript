using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Services.Impl;

public class MethodStorage : IMethodStorage
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