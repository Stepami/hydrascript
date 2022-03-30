using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class ExpressionStatement : Statement
    {
        private readonly Expression _expression;

        public ExpressionStatement(Expression expression)
        {
            _expression = expression;
            expression.Parent = this;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
        }

        protected override string NodeRepresentation() => nameof(ExpressionStatement);

        public override List<Instruction> ToInstructions(int start) => _expression.ToInstructions(start);
    }
}