namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;

public abstract class AccessExpression : Expression
{
    public AccessExpression? Next { get; private set; }

    public AccessExpression? Prev =>
        Parent as AccessExpression;

    public Guid ComputedTypeGuid { get; set; } = Guid.Empty;

    protected AccessExpression(AccessExpression? prev)
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

    public abstract override TReturn Accept<TReturn>(
        IVisitor<IAbstractSyntaxTreeNode, TReturn> visitor);
}