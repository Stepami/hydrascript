using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class WithExpression : Expression
    {
        private readonly IdentifierReference _identifier;
        private readonly ObjectLiteral _object;

        public WithExpression(IdentifierReference identifier, ObjectLiteral @object)
        {
            _identifier = identifier;
            _identifier.Parent = this;

            _object = @object;
            _object.Parent = this;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _object;
        }

        protected override string NodeRepresentation() => $"{_identifier.Id} with";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}