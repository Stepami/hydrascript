using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Visitors.Services;

public interface IValueDtoConverter
{
    IValue Convert(ValueDto dto);
}