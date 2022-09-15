using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Type = Interpreter.Lib.IR.CheckSemantics.Types.Type;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions
{
    public class DotAccess : AccessExpression
    {
        private readonly IdentifierReference _id;

        public DotAccess(IdentifierReference id, AccessExpression prev = null) : base(prev)
        {
            _id = id;
            _id.Parent = this;
        }

        public string Id => _id.Id;

        public override Type Check(Type prev)
        {
            if (prev is ObjectType objectType)
            {
                var fieldType = objectType[_id.Id];
                if (fieldType != null)
                {
                    return HasNext() ? Next.Check(fieldType) : fieldType;
                }

                throw new ObjectAccessException(Segment, objectType, _id.Id);
            }

            return null;
        }

        public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
        {
            yield return _id;
            if (HasNext())
            {
                yield return Next;
            }
        }

        protected override string NodeRepresentation() => ".";

        public override List<Instruction> ToInstructions(int start, string temp)
        {
            if (HasNext())
            {
                var left = "_t" + start;
                var nextInstructions = Next.ToInstructions(start + 1, left);
                nextInstructions.Insert(0,
                    new Simple(left, (new Name(temp), new Constant(_id.Id, _id.Id)), ".", start)
                );
                return nextInstructions;
            }

            return new();
        }
    }
}