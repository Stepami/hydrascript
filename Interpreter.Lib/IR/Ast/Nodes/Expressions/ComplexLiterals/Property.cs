using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals
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

        public void Deconstruct(out string id, out Expression expr)
        {
            id = Id.Id;
            expr = Expression;
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