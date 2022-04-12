using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public abstract class AccessExpression : Expression
    {
        protected AccessExpression next;

        protected AccessExpression(AccessExpression prev)
        {
            if (prev != null)
            {
                Parent = prev;
                prev.next = this;
            }
        }

        public AccessExpression Tail
        {
            get
            {
                var head = this;
                while (head.HasNext())
                {
                    head = head.next;
                }

                return head;
            }
        }

        public abstract Type Check(Type prev);

        public bool HasNext() => next != null;
    }
}