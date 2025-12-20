using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Application.CodeGeneration;

public interface IValueFactory
{
    public IValue Create(ValueDto dto);

    public Name Create(IdentifierReference id);

    public Name Create(string id);
}