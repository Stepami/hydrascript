using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

public class DotAccess : AccessExpression
{
    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression? prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Property;
        if (HasNext())
        {
            yield return Next!;
        }
    }

    protected override string NodeRepresentation() => ".";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}