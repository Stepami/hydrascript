using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

public abstract class LeftHandSideExpression : Expression
{
    public abstract IdentifierReference Id { get; }

    public abstract bool Empty();
}