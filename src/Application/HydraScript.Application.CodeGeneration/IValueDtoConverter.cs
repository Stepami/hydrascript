using HydraScript.Domain.BackEnd;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.CodeGeneration;

public interface IValueDtoConverter
{
    IValue Convert(ValueDto dto);
}