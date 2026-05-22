using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

public interface IFunctionArgument
{
    public string Name { get; }

    public TypeValue TypeValue { get; }

    public ValueDto Info { get; }
}

public record NamedArgument(
    string Name,
    TypeValue TypeValue) : IFunctionArgument
{
    public override string ToString() =>
        $"{Name}: {TypeValue}";

    public ValueDto Info { get; } = ValueDto.NameDto(Name);
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

    public override string ToString() =>
        $"{Name} = {Info.Label}";
}