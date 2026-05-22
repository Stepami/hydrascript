using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

public interface IFunctionArgument
{
    public string Name { get; }

    public TypeValue TypeValue { get; }

    public ValueDto Info { get; }

    public IFunctionArgument DeepClone();
}

public record NamedArgument(
    string Name,
    TypeValue TypeValue) : IFunctionArgument
{
    public override string ToString() =>
        $"{Name}: {TypeValue}";

    public ValueDto Info { get; } = ValueDto.NameDto(Name);

    public IFunctionArgument DeepClone() => this with
    {
        TypeValue = TypeValue.DeepClone()
    };
}

public record DefaultValueArgument : IFunctionArgument
{
    public DefaultValueArgument(string name, Literal literal)
    {
        Name = name;
        TypeValue = literal.Type;
        Info = literal.ToValueDto();
    }

    public string Name { get; }

    public TypeValue TypeValue { get; }

    public ValueDto Info { get; }

    public IFunctionArgument DeepClone() =>
        new DefaultValueArgument(
            Name,
            new Literal(TypeValue.DeepClone(), Info.Value, label: Info.Label));

    public override string ToString() =>
        $"{Name} = {Info.Label}";
}