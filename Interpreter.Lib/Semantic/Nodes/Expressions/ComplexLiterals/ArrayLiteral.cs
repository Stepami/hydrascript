using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;

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
            var instructions = new List<Instruction>
            {
                new CreateArray(start, temp, _expressions.Count)
            };
            var j = 1;
            for (var i = 0; i < _expressions.Count; i++)
            {
                var expr = _expressions[i];
                var index = new Constant(i, i.ToString());
                if (expr is PrimaryExpression prim)
                {
                    instructions.Add(new IndexAssignment(temp, (index, prim.ToValue()), start + j));
                    j++;
                }
                else
                {
                    var propInstructions = expr.ToInstructions(start + j, "_t" + (start + j));
                    j += propInstructions.Count;
                    var left = propInstructions.OfType<Simple>().Last().Left;
                    propInstructions.Add(new IndexAssignment(temp, (index, new Name(left)), start + j));
                    j++;
                    instructions.AddRange(propInstructions);
                }
            }

            return instructions;
        }
    }
}