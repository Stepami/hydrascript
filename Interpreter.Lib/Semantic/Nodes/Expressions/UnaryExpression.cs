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
        private readonly string @operator;

        private readonly Expression expression;

        public UnaryExpression(string @operator, Expression expression)
        {
            this.@operator = @operator;

            this.expression = expression;
            this.expression.Parent = this;
        }

        internal override Type NodeCheck()
        {
            var eType = expression.NodeCheck();
            Type retType;
            if (eType.Equals(TypeUtils.JavaScriptTypes.Number) && @operator == "-")
            {
                retType = TypeUtils.JavaScriptTypes.Number;
            }
            else if (eType.Equals(TypeUtils.JavaScriptTypes.Boolean) && @operator == "!")
            {
                retType = TypeUtils.JavaScriptTypes.Boolean;
            }
            else throw new UnsupportedOperation(Segment, eType, @operator);

            return retType;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return expression;
        }

        protected override string NodeRepresentation() => @operator;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();

            (IValue left, IValue right) right = (null, null);
            if (expression.Primary())
            {
                right.right = ((PrimaryExpression) expression).ToValue();
            }
            else
            {
                instructions.AddRange(expression.ToInstructions(start, temp));
                right.right = new Name(instructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
            }

            var number = instructions.Any() ? instructions.Last().Number + 1 : start;

            instructions.Add(new ThreeAddressCodeInstruction(
                temp + number, right, @operator, number
            ));
            return instructions;
        }
    }
}