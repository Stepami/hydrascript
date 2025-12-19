using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal sealed class ExplicitCastValidator : IExplicitCastValidator
{
    private static readonly Type Boolean = "boolean";
    private static readonly Type Number = "number";
    private static readonly Type String = "string";
    private static readonly Any Any = new();

    private readonly Dictionary<Type, List<Type>> _allowedConversions = new()
    {
        { String, [Any] },
        { Number, [String, Boolean] },
        { Boolean, [String, Number] },
    };

    public bool IsAllowed(Type from, Type to)
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