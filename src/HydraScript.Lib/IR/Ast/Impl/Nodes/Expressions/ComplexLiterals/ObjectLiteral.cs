namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ObjectLiteral : ComplexLiteral
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
}