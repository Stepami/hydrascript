using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class CastAsExpression : Expression
    {
        private readonly Expression expression;
        private readonly Type cast;

        public CastAsExpression(Expression expression, Type cast)
        {
            this.expression = expression;
            this.expression.Parent = this;

            this.cast = cast;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return expression;
        }

        protected override string NodeRepresentation() => $"as {cast}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}