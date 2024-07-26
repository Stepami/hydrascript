using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Visitors.Services.Impl;

public class ValueDtoConverter : IValueDtoConverter
{
    public IValue Convert(ValueDto dto) =>
        dto switch
        {
            { Type: ValueDtoType.Constant, Label: not null } =>
                new Constant(dto.Value, dto.Label),
            { Type: ValueDtoType.Name, Name: not null } =>
                new Name(dto.Name),
            _ => throw new ArgumentOutOfRangeException(nameof(dto))
        };
}