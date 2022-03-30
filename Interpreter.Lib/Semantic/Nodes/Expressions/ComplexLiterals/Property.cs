using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class Property : Expression
    {
        private readonly IdentifierReference _id;
        private readonly Expression _expression;

        public Property(IdentifierReference id, Expression expression)
        {
            _id = id;
            _id.Parent = this;

            _expression = expression;
            _expression.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _id;
            yield return _expression;
        }

        protected override string NodeRepresentation() => ":";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}