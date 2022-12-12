using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions
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
                if (_left is MemberExpression member && member.Any())
                {
                    var i = start + lInstructions.Count;
                    var dest = "_t" + i;
                    var src = lInstructions.Any()
                        ? lInstructions.OfType<Simple>().Last().Left
                        : member.Id;
                    var instruction = member.AccessChain.Tail switch
                    {
                        DotAccess dot => new Simple(dest, (new Name(src), new Constant(dot.Id, dot.Id)), ".", i),
                        IndexAccess index => new Simple(dest, (new Name(src), index.Expression.ToValue()), "[]", i),
                        _ => throw new NotImplementedException()
                    };
                    lInstructions.Add(instruction);
                }
                newRight.left = new Name(lInstructions.OfType<Simple>().Last().Left);
            }

            if (_right.Primary())
            {
                newRight.right = ((PrimaryExpression) _right).ToValue();
            }
            else
            {
                var c = _left.Primary()
                    ? start
                    : lInstructions.Last().Number + 1;
                rInstructions.AddRange(_right.ToInstructions(c, temp));
                if (_right is MemberExpression member && member.Any())
                {
                    var i = c + rInstructions.Count;
                    var dest = "_t" + i;
                    var src = rInstructions.Any()
                        ? rInstructions.OfType<Simple>().Last().Left
                        : member.Id;
                    var instruction = member.AccessChain.Tail switch
                    {
                        DotAccess dot => new Simple(dest, (new Name(src), new Constant(dot.Id, dot.Id)), ".", i),
                        IndexAccess index => new Simple(dest, (new Name(src), index.Expression.ToValue()), "[]", i),
                        _ => throw new NotImplementedException()
                    };
                    rInstructions.Add(instruction);
                }
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