using System;
using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Interpreter.Lib.VM.Values;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Nodes.Expressions.AccessExpressions
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
                yield return next;
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
                    return HasNext() ? next.Check(elemType) : elemType;
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
                    var nextInstructions = next.ToInstructions(start + 1, left);
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