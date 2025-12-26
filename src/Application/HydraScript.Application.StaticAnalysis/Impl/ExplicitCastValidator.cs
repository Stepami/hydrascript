using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal sealed class ExplicitCastValidator(IJavaScriptTypesProvider typesProvider) : IExplicitCastValidator
{
    private readonly Dictionary<Type, List<Type>> _allowedConversions = new()
    {
        { typesProvider.String, [new Any()] },
        { typesProvider.Number, [typesProvider.String, typesProvider.Boolean] },
        { typesProvider.Boolean, [typesProvider.String, typesProvider.Number] },
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