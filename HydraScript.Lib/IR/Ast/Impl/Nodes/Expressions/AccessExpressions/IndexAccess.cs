using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

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

    protected override string NodeRepresentation() => "[]";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}