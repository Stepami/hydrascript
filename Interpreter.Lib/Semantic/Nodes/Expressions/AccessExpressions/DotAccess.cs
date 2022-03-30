using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public class DotAccess : AccessExpression
    {
        private readonly IdentifierReference _id;

        public DotAccess(IdentifierReference id, AccessExpression prev = null) : base(prev)
        {
            _id = id;
            _id.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _id;
            if (HasNext())
            {
                yield return next;
            }
        }

        protected override string NodeRepresentation() => ".";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}