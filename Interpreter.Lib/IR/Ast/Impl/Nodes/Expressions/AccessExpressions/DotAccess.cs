using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;

public class DotAccess : AccessExpression
{
    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Property;
        if (HasNext())
        {
            yield return Next;
        }
    }

    protected override string NodeRepresentation() => ".";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}