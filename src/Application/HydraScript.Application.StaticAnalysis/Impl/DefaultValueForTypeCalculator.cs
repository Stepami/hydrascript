using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class DefaultValueForTypeCalculator : IDefaultValueForTypeCalculator
{
    public object? GetDefaultValueForType(Type type)
    {
        if (type is NullableType)
            return null;
        if (type.Equals("boolean"))
            return false;
        if (type.Equals("number"))
            return 0;
        if (type.Equals("string"))
            return string.Empty;
        if (type.Equals("void"))
            return new object();
        if (type.Equals(new NullType()))
            return null;
        if (type is ArrayType)
            return new List<object>();

        return new object();
    }
}