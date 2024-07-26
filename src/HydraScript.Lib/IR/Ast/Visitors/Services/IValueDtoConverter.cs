using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Visitors.Services;

public interface IValueDtoConverter
{
    IValue Convert(ValueDto dto);
}