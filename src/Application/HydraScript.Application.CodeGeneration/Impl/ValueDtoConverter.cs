using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.CodeGeneration.Impl;

internal class ValueDtoConverter : IValueDtoConverter
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