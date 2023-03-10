using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions.AccessExpressions;

public class DotAccess : AccessExpression
{
    public IdentifierReference Property { get; }

    public DotAccess(IdentifierReference property, AccessExpression prev = null) : base(prev)
    {
        Property = property;
        Property.Parent = this;
    }

    public override Type Check(Type prev)
    {
        if (prev is ObjectType objectType)
        {
            var fieldType = objectType[Property.Name];
            if (fieldType != null)
            {
                return HasNext() ? Next.Check(fieldType) : fieldType;
            }

            throw new ObjectAccessException(Segment, objectType, Property.Name);
        }

        return null;
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