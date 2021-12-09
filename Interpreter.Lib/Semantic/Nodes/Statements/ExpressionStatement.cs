using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class ExpressionStatement : Statement
    {
        private readonly Expression expression;

        public ExpressionStatement(Expression expression)
        {
            this.expression = expression;
            expression.Parent = this;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return expression;
        }

        protected override string NodeRepresentation() => nameof(ExpressionStatement);

        public override List<Instruction> ToInstructions(int start) => expression.ToInstructions(start);
    }
}