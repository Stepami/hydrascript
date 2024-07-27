using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.CodeGeneration.Services;

public interface IValueDtoConverter
{
    IValue Convert(ValueDto dto);
}