using System;
using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Type = Interpreter.Lib.IR.CheckSemantics.Types.Type;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions
{
    public class IndexAccess : AccessExpression
    {
        private readonly Expression _expression;

        public IndexAccess(Expression expression, AccessExpression prev = null) : base(prev)
        {
            _expression = expression;
            _expression.Parent = this;
        }
        
        public PrimaryExpression Expression => _expression as PrimaryExpression;

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _expression;
            if (HasNext())
            {
                yield return Next;
            }
        }

        public override Type Check(Type prev)
        {
            if (prev is ArrayType arrayType)
            {
                var indexType = _expression.NodeCheck();
                if (indexType.Equals(TypeUtils.JavaScriptTypes.Number))
                {
                    var elemType = arrayType.Type;
                    return HasNext() ? Next.Check(elemType) : elemType;
                }

                throw new ArrayAccessException(Segment, indexType);
            }

            return null;
        }

        protected override string NodeRepresentation() => "[]";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            if (HasNext())
            {
                if (_expression is PrimaryExpression prim)
                {
                    var left = "_t" + start;
                    var nextInstructions = Next.ToInstructions(start + 1, left);
                    nextInstructions.Insert(0,
                        new Simple(left, (new Name(temp), prim.ToValue()), "[]", start)
                    );
                    return nextInstructions;
                }

                throw new NotImplementedException();
            }

            return new();
        }
    }
}