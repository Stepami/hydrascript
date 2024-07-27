using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Services.Impl;

public class JavaScriptTypesProvider : IJavaScriptTypesProvider
{
    private readonly HashSet<Type> _types =
    [
        "number",
        "boolean",
        "string",
        new NullType(),
        "undefined",
        "void"
    ];

    public IEnumerable<Type> GetDefaultTypes() => _types;

    public bool Contains(Type type) => _types.Contains(type);
}