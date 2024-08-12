using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

public abstract class LeftHandSideExpression : Expression
{
    public abstract IdentifierReference Id { get; }

    public abstract bool Empty();
}