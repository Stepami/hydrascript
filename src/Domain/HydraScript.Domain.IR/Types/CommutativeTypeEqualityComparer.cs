using System.Diagnostics.CodeAnalysis;

namespace HydraScript.Domain.IR.Types;

public readonly ref struct CommutativeTypeEqualityComparer : IEqualityComparer<Type>
{
    public bool Equals(Type? x, Type? y) =>
        x?.Equals(y) != false || y?.Equals(x) != false;

    public int GetHashCode(Type obj) => obj.GetHashCode();
}