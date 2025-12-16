namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

public record ValueDto(
    ValueDtoType Type,
    string? Name,
    object? Value,
    string? Label)
{
    public static ValueDto ConstantDto(object? value, string label) =>
        new(ValueDtoType.Constant, Name: null, value, label);

    public static ValueDto NameDto(string name) =>
        new(ValueDtoType.Name, name, Value: null, Label: null);

    public static ValueDto EnvDto(string name) =>
        new(ValueDtoType.Env, name, Value: null, Label: null);
};

public enum ValueDtoType
{
    Constant = 1,
    Name = 2,
    Env = 3
}