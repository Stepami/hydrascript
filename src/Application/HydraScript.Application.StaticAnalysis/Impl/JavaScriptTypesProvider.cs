using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class JavaScriptTypesProvider : IJavaScriptTypesProvider
{
    private readonly HashSet<Type> _types;

    public JavaScriptTypesProvider()
    {
        _types =
        [
            Number,
            Boolean,
            String,
            Null,
            Undefined,
            Void
        ];
    }

    public Type Number { get; } = "number";

    public Type Boolean { get; } = "boolean";

    public Type String { get; } = new StringType();

    public Type Null { get; } = new NullType();

    public Type Undefined { get; } = "undefined";

    public Type Void { get; } = "void";

    public IEnumerable<Type> GetDefaultTypes() => _types;

    public bool Contains(Type type) => _types.Contains(type);
}