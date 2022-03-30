using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.ComplexLiterals
{
    public class ArrayLiteral : Expression
    {
        private readonly List<Expression> _expressions;

        public ArrayLiteral(IEnumerable<Expression> expressions)
        {
            _expressions = new List<Expression>(expressions);
            _expressions.ForEach(expr => expr.Parent = this);
        }
        
        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
            _expressions.GetEnumerator();

        protected override string NodeRepresentation() => "[]";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            throw new System.NotImplementedException();
        }
    }
}