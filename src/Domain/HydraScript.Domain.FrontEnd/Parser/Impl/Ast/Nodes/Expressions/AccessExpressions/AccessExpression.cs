namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;

public abstract class AccessExpression : Expression
{
    protected AccessExpression? Next { get; private set; }

    public AccessExpression? Prev => Parent as AccessExpression;

    protected AccessExpression(AccessExpression? prev)
    {
        if (prev is not null)
        {
            Parent = prev;
            prev.Next = this;
        }
    }

    public bool HasPrev() => Prev is not null;

    public abstract override TReturn Accept<TReturn>(
        IVisitor<IAbstractSyntaxTreeNode, TReturn> visitor);

    public abstract override AccessExpression Clone();
}