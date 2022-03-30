using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class UnaryExpression : Expression
    {
        private readonly string _operator;

        private readonly Expression _expression;

        public UnaryExpression(string @operator, Expression expression)
        {
            _operator = @operator;

            _expression = expression;
            _expression.Parent = this;
        }

        internal override Type NodeCheck()
        {
            var eType = _expression.NodeCheck();
            Type retType;
            if (eType.Equals(TypeUtils.JavaScriptTypes.Number) && _operator == "-")
            {
                retType = TypeUtils.JavaScriptTypes.Number;
            }
            else if (eType.Equals(TypeUtils.JavaScriptTypes.Boolean) && _operator == "!")
            {
                retType = TypeUtils.JavaScriptTypes.Boolean;
            }
            else throw new UnsupportedOperation(Segment, eType, _operator);

            return retType;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
        }

        protected override string NodeRepresentation() => _operator;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();

            (IValue left, IValue right) right = (null, null);
            if (_expression.Primary())
            {
                right.right = ((PrimaryExpression) _expression).ToValue();
            }
            else
            {
                instructions.AddRange(_expression.ToInstructions(start, temp));
                right.right = new Name(instructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
            }

            var number = instructions.Any() ? instructions.Last().Number + 1 : start;

            instructions.Add(new ThreeAddressCodeInstruction(
                temp + number, right, _operator, number
            ));
            return instructions;
        }
    }
}