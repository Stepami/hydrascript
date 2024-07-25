using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public class ObjectLiteral : ComplexLiteral
{
    public List<Property> Properties { get; }

    public ObjectLiteral(IEnumerable<Property> properties)
    {
        Properties = new List<Property>(properties);
        Properties.ForEach(prop => prop.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Properties.GetEnumerator();

    protected override string NodeRepresentation() => "{}";

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}