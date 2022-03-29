using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class Property : Expression
    {
        private readonly IdentifierReference id;
        private readonly Expression expression;

        public Property(IdentifierReference id, Expression expression)
        {
            this.id = id;
            this.id.Parent = this;

            this.expression = expression;
            this.expression.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return id;
            yield return expression;
        }

        protected override string NodeRepresentation() => ":";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}