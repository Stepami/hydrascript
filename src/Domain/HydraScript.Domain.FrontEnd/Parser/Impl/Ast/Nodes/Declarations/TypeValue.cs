using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

public abstract record TypeValue : IVisitable<TypeValue>
{
    public Scope Scope { get; set; } = null!;
    public abstract TReturn Accept<TReturn>(IVisitor<TypeValue, TReturn> visitor);
    public abstract TypeValue DeepClone();
}

[AutoVisitable<TypeValue>]
public partial record TypeIdentValue(IdentifierReference TypeId) : TypeValue
{
    public static TypeIdentValue String => new(new IdentifierReference("string"));
    public static TypeIdentValue Number => new(new IdentifierReference("number"));
    public static TypeIdentValue Boolean => new(new IdentifierReference("boolean"));
    public static TypeIdentValue Null => new(new IdentifierReference("null"));
    public static TypeIdentValue Undefined => new(new IdentifierReference("undefined"));

    public override string ToString() => TypeId;

    public override TypeValue DeepClone() => new TypeIdentValue(TypeId.Clone());
}

[AutoVisitable<TypeValue>]
public partial record ArrayTypeValue(TypeValue TypeValue) : TypeValue
{
    public override string ToString() => $"{TypeValue}[]";

    public override TypeValue DeepClone() => new ArrayTypeValue(TypeValue.DeepClone());
}

[AutoVisitable<TypeValue>]
public partial record NullableTypeValue(TypeValue TypeValue) : TypeValue
{
    public override string ToString() => $"{TypeValue}?";

    public override TypeValue DeepClone() => new NullableTypeValue(TypeValue.DeepClone());
}

public record PropertyTypeValue(
    string Key,
    TypeValue TypeValue)
{
    public override string ToString() =>
        $"{Key}: {TypeValue}";

    public PropertyTypeValue DeepClone() => this with
    {
        TypeValue = TypeValue.DeepClone()
    };
}

[AutoVisitable<TypeValue>]
public partial record ObjectTypeValue(List<PropertyTypeValue> Properties) : TypeValue
{
    public override string ToString() => $"{{{string.Join(';', Properties)}}}";

    public override TypeValue DeepClone() => new ObjectTypeValue(
        Properties
            .Select(p => p.DeepClone())
            .ToList());
}