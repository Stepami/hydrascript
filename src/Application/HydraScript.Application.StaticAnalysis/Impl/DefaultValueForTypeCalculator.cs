using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class DefaultValueForTypeCalculator(IJavaScriptTypesProvider typesProvider) : IDefaultValueForTypeCalculator
{
    public object? GetDefaultValueForType(Type type)
    {
        if (type is NullableType)
            return null;
        if (type.Equals(typesProvider.Boolean))
            return false;
        if (type.Equals(typesProvider.Number))
            return 0;
        if (type.Equals(typesProvider.String))
            return string.Empty;
        if (type.Equals(typesProvider.Void))
            return new object();
        if (type.Equals(typesProvider.Null))
            return null;
        if (type is ArrayType)
            return new List<object>();

        return new object();
    }
}