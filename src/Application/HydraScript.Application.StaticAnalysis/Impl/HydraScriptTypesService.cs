using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class HydraScriptTypesService : IHydraScriptTypesService
{
    private readonly HashSet<Type> _types;
    private readonly Dictionary<Type, List<Type>> _allowedConversions;

    public HydraScriptTypesService()
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
        _allowedConversions = new()
        {
            [String] = [new Any()],
            [Number] = [String, Boolean],
            [Boolean] = [String, Number],
        };
    }

    public Type Number => NumberType.Instance;

    public Type Boolean => BooleanType.Instance;

    public Type String => StringType.Instance;

    private Type Null => NullType.Instance;

    public Type Undefined => "undefined";

    public Type Void => "void";

    public IEnumerable<Type> GetDefaultTypes() => _types;

    public bool Contains(Type type) => _types.Contains(type);

    public object? GetDefaultValueForType(Type type)
    {
        if (type is NullableType)
            return null;
        if (type.Equals(Boolean))
            return false;
        if (type.Equals(Number))
            return 0;
        if (type.Equals(String))
            return string.Empty;
        if (type.Equals(Void))
            return new object();
        if (type.Equals(Null))
            return null;
        if (type is ArrayType)
            return new List<object>();

        return new object();
    }

    public bool IsExplicitCastAllowed(Type from, Type to)
    {
        var typeEqualityComparer = default(CommutativeTypeEqualityComparer);

        if (typeEqualityComparer.Equals(from, to))
            return true;

        if (!_allowedConversions.TryGetValue(to, out var allowedFrom))
            return false;

        for (var i = 0; i < allowedFrom.Count; i++)
        {
            if (typeEqualityComparer.Equals(allowedFrom[i], from))
                return true;
        }

        return false;
    }
}