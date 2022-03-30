using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Types;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class CastAsExpression : Expression
    {
        private readonly Expression _expression;
        private readonly Type _cast;

        public CastAsExpression(Expression expression, Type cast)
        {
            _expression = expression;
            _expression.Parent = this;

            _cast = cast;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
        }

        protected override string NodeRepresentation() => $"as {_cast}";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}