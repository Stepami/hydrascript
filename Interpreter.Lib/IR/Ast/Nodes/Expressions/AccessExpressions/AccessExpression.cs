namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;

public abstract class AccessExpression : Expression
{
    public AccessExpression Next { get; private set; }

    protected AccessExpression(AccessExpression prev)
    {
        if (prev != null)
        {
            Parent = prev;
            prev.Next = this;
        }
    }

    public AccessExpression Tail
    {
        get
        {
            var head = this;
            while (head.HasNext())
            {
                head = head.Next;
            }

            return head;
        }
    }

    public abstract Type Check(Type prev);

    public bool HasNext() => Next != null;
}