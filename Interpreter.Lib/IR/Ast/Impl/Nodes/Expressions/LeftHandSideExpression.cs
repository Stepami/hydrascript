using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public abstract class LeftHandSideExpression : Expression
{
    public abstract IdentifierReference Id { get; }

    public abstract bool Empty();
}