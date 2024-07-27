using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

public abstract record TypeValue : IVisitable<TypeValue>
{
    public Scope Scope { get; set; } = default!;
    public abstract TReturn Accept<TReturn>(IVisitor<TypeValue, TReturn> visitor);
}

[AutoVisitable<TypeValue>]
public partial record TypeIdentValue(IdentifierReference TypeId) : TypeValue
{
    public override string ToString() => TypeId;
}

[AutoVisitable<TypeValue>]
public partial record ArrayTypeValue(TypeValue TypeValue) : TypeValue
{
    public override string ToString() => $"{TypeValue}[]";
}

[AutoVisitable<TypeValue>]
public partial record NullableTypeValue(TypeValue TypeValue) : TypeValue
{
    public override string ToString() => $"{TypeValue}?";
}

public record PropertyTypeValue(
    string Key,
    TypeValue TypeValue)
{
    public override string ToString() =>
        $"{Key}: {TypeValue}";
}

[AutoVisitable<TypeValue>]
public partial record ObjectTypeValue(
    IEnumerable<PropertyTypeValue> Properties) : TypeValue
{
    public override string ToString() =>
        $"{{{string.Join(';', Properties)}}}";
}