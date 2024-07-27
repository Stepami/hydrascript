using HydraScript.Lib.IR.CheckSemantics.Types;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

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