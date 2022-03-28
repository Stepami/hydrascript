namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public abstract class AccessExpression : Expression
    {
        protected readonly AccessExpression prev;
        protected AccessExpression next;

        protected AccessExpression(AccessExpression prev)
        {
            this.prev = prev;
            if (prev != null)
            {
                Parent = prev;
                this.prev.next = this;
            }
        }

        public bool HasNext() => next != null;
    }
}