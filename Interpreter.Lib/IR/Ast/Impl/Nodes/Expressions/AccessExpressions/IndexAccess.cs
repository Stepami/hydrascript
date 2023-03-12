using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

public class IndexAccess : AccessExpression
{
    public Expression Index { get; }

    public IndexAccess(Expression index, AccessExpression prev = null) : base(prev)
    {
        Index = index;
        Index.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Index;
        if (HasNext())
        {
            yield return Next;
        }
    }

    public override Type Check(Type prev)
    {
        if (prev is ArrayType arrayType)
        {
            var indexType = Index.NodeCheck();
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

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}