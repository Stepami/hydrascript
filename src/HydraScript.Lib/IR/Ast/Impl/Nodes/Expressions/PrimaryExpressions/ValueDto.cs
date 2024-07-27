namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public record ValueDto(
    ValueDtoType Type,
    string? Name,
    object? Value,
    string? Label)
{
    public static ValueDto NameDto(string name) =>
        new(ValueDtoType.Name, name, Value: null, Label: null);

    public static ValueDto ConstantDto(object? value, string label) =>
        new(ValueDtoType.Constant, Name: null, value, label);
};

public enum ValueDtoType
{
    Constant = 1,
    Name = 2
}