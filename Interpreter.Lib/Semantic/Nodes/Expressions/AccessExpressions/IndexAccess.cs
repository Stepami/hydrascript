using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.Semantic.Exceptions;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
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
            throw new System.NotImplementedException();
        }
    }
}