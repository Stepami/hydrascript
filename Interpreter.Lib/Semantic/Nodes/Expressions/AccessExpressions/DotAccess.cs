using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
{
    public class DotAccess : AccessExpression
    {
        private IdentifierReference id;

        public DotAccess(IdentifierReference id)
        {
            this.id = id;
            this.id.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return id;
        }

        protected override string NodeRepresentation() => ".";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}