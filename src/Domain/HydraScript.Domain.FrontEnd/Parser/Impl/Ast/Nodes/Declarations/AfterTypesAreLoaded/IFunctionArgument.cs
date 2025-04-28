using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;

public interface IFunctionArgument
{
    public string Name { get; }

    public TypeValue TypeValue { get; }
}

public record NamedArgument(
    string Name,
    TypeValue TypeValue) : IFunctionArgument
{
    public override string ToString() =>
        $"{Name}: {TypeValue}";
}

public record DefaultValueArgument(
    string Name,
    Literal Value) : IFunctionArgument
{
    public TypeValue TypeValue { get; } = Value.Type;

    public override string ToString() =>
        $"{Name} = {Value}";
}