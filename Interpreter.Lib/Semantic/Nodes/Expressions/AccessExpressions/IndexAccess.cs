using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public class IndexAccess : AccessExpression
    {
        private readonly Expression _expression;

        public IndexAccess(Expression expression, AccessExpression prev = null) : base(prev)
        {
            _expression = expression;
            _expression.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
            if (HasNext())
            {
                yield return next;
            }
        }

        public override Type Check(Type prev)
        {
            throw new System.NotImplementedException();
        }

        protected override string NodeRepresentation() => "[]";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}