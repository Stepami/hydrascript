using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

public abstract class AccessExpression : Expression
{
    public AccessExpression Next { get; private set; }

    public AccessExpression Prev =>
        Parent as AccessExpression;

    public Type ComputedType { get; set; }

    protected AccessExpression(AccessExpression prev)
    {
        if (prev is not null)
        {
            Parent = prev;
            prev.Next = this;
        }
    }

    public bool HasNext() =>
        Next is not null;

    public bool HasPrev() =>
        Prev is not null;

    public abstract override Type Accept(SemanticChecker visitor);
}