using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class Property : Expression
    {
        public IdentifierReference Id { get; }
        public Expression Expression { get; }

        public Property(IdentifierReference id, Expression expression)
        {
            Id = id;
            Id.Parent = this;

            Expression = expression;
            Expression.Parent = this;
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return Id;
            yield return Expression;
        }

        protected override string NodeRepresentation() => ":";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}