using System;
using System.Collections.Generic;
using System.Linq;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions
{
    public class BinaryExpression : Expression
    {
        private readonly Expression _left;

        private readonly string _operator;

        private readonly Expression _right;

        public BinaryExpression(Expression left, string @operator, Expression right)
        {
            _left = left;
            _left.Parent = this;

            _operator = @operator;

            _right = right;
            _right.Parent = this;
        }

        internal override Type NodeCheck()
        {
            var lType = _left.NodeCheck();
            var rType = _right.NodeCheck();
            Type retType = null;
            if (_operator != "::" && !lType.Equals(rType))
            {
                throw new IncompatibleTypesOfOperands(Segment, lType, rType);
            }

            switch (_operator)
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
                    else throw new UnsupportedOperation(Segment, lType, _operator);

                    break;
                case "-":
                case "*":
                case "/":
                case "%":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Number))
                    {
                        retType = TypeUtils.JavaScriptTypes.Number;
                    }
                    else throw new UnsupportedOperation(Segment, lType, _operator);

                    break;
                case "||":
                case "&&":
                    if (lType.Equals(TypeUtils.JavaScriptTypes.Boolean))
                    {
                        retType = TypeUtils.JavaScriptTypes.Boolean;
                    }
                    else throw new UnsupportedOperation(Segment, lType, _operator);

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
                    else throw new UnsupportedOperation(Segment, lType, _operator);

                    break;
                case "++":
                    if (lType is ArrayType && rType is ArrayType)
                    {
                        retType = lType;
                    }
                    else throw new UnsupportedOperation(Segment, lType, _operator);

                    break;
                case "::":
                    if (!(lType is ArrayType))
                    {
                        throw new UnsupportedOperation(Segment, lType, _operator);
                    }
                    if (rType.Equals(TypeUtils.JavaScriptTypes.Number))
                    {
                        retType = TypeUtils.JavaScriptTypes.Void;
                    }
                    else throw new ArrayAccessException(Segment, rType);

                    break;
            }

            return retType;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _left;
            yield return _right;
        }

        protected override string NodeRepresentation() => _operator;

        public override List<Instruction> ToInstructions(int start)
        {
            if (_left is IdentifierReference arr && _right.Primary() && _operator == "::")
            {
                return new List<Instruction>
                {
                    new RemoveFromArray(start, arr.Id, ((PrimaryExpression) _right).ToValue())
                };
            }

            throw new NotImplementedException();
        }

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            var instructions = new List<Instruction>();
            (IValue left, IValue right) newRight = (null, null);

            var lInstructions = new List<Instruction>();
            var rInstructions = new List<Instruction>();

            if (_left.Primary())
            {
                newRight.left = ((PrimaryExpression) _left).ToValue();
            }
            else
            {
                lInstructions.AddRange(_left.ToInstructions(start, temp));
                newRight.left = new Name(lInstructions.OfType<Simple>().Last().Left);
            }

            if (_right.Primary())
            {
                newRight.right = ((PrimaryExpression) _right).ToValue();
            }
            else
            {
                rInstructions.AddRange(_right.ToInstructions(
                    _left.Primary()
                        ? start
                        : lInstructions.Last().Number + 1,
                    temp
                ));
                newRight.right = new Name(rInstructions.OfType<Simple>().Last().Left);
            }

            instructions.AddRange(lInstructions);
            instructions.AddRange(rInstructions);

            var number = instructions.Any() ? instructions.Last().Number + 1 : start;

            instructions.Add
            (
                new Simple(
                    temp + number,
                    newRight, _operator, number
                )
            );
            return instructions;
        }
    }
}