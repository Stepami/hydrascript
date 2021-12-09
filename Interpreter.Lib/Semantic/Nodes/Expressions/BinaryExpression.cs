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
    public class BinaryExpression : Expression
    {
        private readonly Expression left;

        private readonly string @operator;

        private readonly Expression right;

        public BinaryExpression(Expression left, string @operator, Expression right)
        {
            this.left = left;
            this.left.Parent = this;

            this.@operator = @operator;

            this.right = right;
            this.right.Parent = this;
        }

        internal override Type NodeCheck()
        {
            var lType = left.NodeCheck();
            var rType = right.NodeCheck();
            Type retType = null;
            if (!lType.Equals(rType))
            {
                throw new IncompatibleTypesOfOperands(Segment, lType, rType);
            }

            switch (@operator)
            {
                case "+":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                    {
                        retType = TypeUtils.JavaScriptTypes.Number;
                    }
                    else if (lType.Equals(TypeUtils.JavaScriptTypes.String))
                    {
                        retType = TypeUtils.JavaScriptTypes.String;
                    }
                    else throw new UnsupportedOperation(Segment, lType, @operator);

                    break;
                case "-":
                case "*":
                case "/":
                case "%":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                    {
                        retType = TypeUtils.JavaScriptTypes.Number;
                    }
                    else throw new UnsupportedOperation(Segment, lType, @operator);

                    break;
                case "||":
                case "&&":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Boolean))
                    {
                        retType = TypeUtils.JavaScriptTypes.Boolean;
                    }
                    else throw new UnsupportedOperation(Segment, lType, @operator);

                    break;
                case "==":
                case "!=":
                    retType = TypeUtils.JavaScriptTypes.Boolean;
                    break;
                case ">":
                case ">=":
                case "<":
                case "<=":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                    {
                        retType = TypeUtils.JavaScriptTypes.Boolean;
                    }
                    else throw new UnsupportedOperation(Segment, lType, @operator);

                    break;
            }

            return retType;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return left;
            yield return right;
        }

        protected override string NodeRepresentation() => @operator;

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();
            (IValue left, IValue right) newRight = (null, null);

            var lInstructions = new List<Instruction>();
            var rInstructions = new List<Instruction>();

            if (left.Primary())
            {
                newRight.left = ((PrimaryExpression) left).ToValue();
            }
            else
            {
                lInstructions.AddRange(left.ToInstructions(start, temp));
                newRight.left = new Name(lInstructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
            }

            if (right.Primary())
            {
                newRight.right = ((PrimaryExpression) right).ToValue();
            }
            else
            {
                rInstructions.AddRange(right.ToInstructions(
                    left.Primary()
                        ? start
                        : lInstructions.Last().Number + 1,
                    temp
                ));
                newRight.right = new Name(rInstructions.OfType<ThreeAddressCodeInstruction>().Last().Left);
            }

            instructions.AddRange(lInstructions);
            instructions.AddRange(rInstructions);

            var number = instructions.Any() ? instructions.Last().Number + 1 : start;

            instructions.Add
            (
                new ThreeAddressCodeInstruction(
                    temp + number,
                    newRight, @operator, number
                )
            );
            return instructions;
        }
    }
}