using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public class IdentifierReference : PrimaryExpression
{
    public string Name { get; }

    public IdentifierReference(string name)
    {
        Name = name;
    }

    protected override string NodeRepresentation() => Name;

    public override IValue ToValue() => new Name(Name);

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public static implicit operator string(IdentifierReference identifierReference) =>
        identifierReference.Name;
}