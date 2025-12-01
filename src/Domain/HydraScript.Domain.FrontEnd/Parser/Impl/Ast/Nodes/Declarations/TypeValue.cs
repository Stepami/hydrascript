using Cysharp.Text;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

public abstract record TypeValue : IVisitable<TypeValue>
{
    public Scope Scope { get; set; } = null!;
    public abstract TReturn Accept<TReturn>(IVisitor<TypeValue, TReturn> visitor);
}

[AutoVisitable<TypeValue>]
public partial record TypeIdentValue(IdentifierReference TypeId) : TypeValue
{
    public static TypeValue String => new TypeIdentValue(new IdentifierReference("string"));
    public static TypeValue Number => new TypeIdentValue(new IdentifierReference("number"));
    public static TypeValue Boolean => new TypeIdentValue(new IdentifierReference("boolean"));
    public static TypeValue Null => new TypeIdentValue(new IdentifierReference("null"));
    public static TypeValue Undefined => new TypeIdentValue(new IdentifierReference("undefined"));

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
public partial record ObjectTypeValue(List<PropertyTypeValue> Properties) : TypeValue
{
    public override string ToString() =>
        ZString.Concat('{', ZString.Join(';', Properties), '}');
}