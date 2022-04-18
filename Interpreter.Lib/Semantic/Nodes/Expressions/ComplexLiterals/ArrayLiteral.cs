using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;

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

        internal override Type NodeCheck()
        {
            if (_expressions.Any())
            {
                var type = _expressions.First().NodeCheck();
                if (_expressions.All(e => e.NodeCheck().Equals(type)))
                {
                    return new ArrayType(type);
                }

                throw new WrongArrayLiteralDeclaration(Segment, type);
            }

            return TypeUtils.JavaScriptTypes.Undefined;
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