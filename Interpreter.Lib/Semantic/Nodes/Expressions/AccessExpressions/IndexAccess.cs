using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public class IndexAccess : AccessExpression
    {
        private readonly Expression expression;

        public IndexAccess(Expression expression, AccessExpression prev = null) : base(prev)
        {
            this.expression = expression;
            this.expression.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return expression;
            if (HasNext())
            {
                yield return next;
            }
        }

        protected override string NodeRepresentation() => "[]";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}