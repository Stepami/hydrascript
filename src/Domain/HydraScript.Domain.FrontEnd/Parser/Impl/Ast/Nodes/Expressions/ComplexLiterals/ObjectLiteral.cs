namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ObjectLiteral : ComplexLiteral
{
    private readonly List<Property> _properties;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _properties;

    public IReadOnlyList<Property> Properties => _properties;

    public override string Id
    {
        get
        {
            if (Parent is AssignmentExpression assignment) 
                return assignment.Destination.Id;

            if (Parent is WithExpression{Parent:AssignmentExpression withAssignment})
                return withAssignment.Destination.Id;

            return NullId;
        }
    }

    public ObjectLiteral(List<Property> properties)
    {
        _properties = properties;
        _properties.ForEach(prop => prop.Parent = this);
    }

    /// <summary>Стратегия "блока" - углубление скоупа</summary>
    /// <param name="scope">Новый скоуп</param>
    public override void InitScope(Scope? scope = null)
    {
        ArgumentNullException.ThrowIfNull(scope);
        Scope = scope;
        Scope.AddOpenScope(Parent.Scope);
    }

    protected override string NodeRepresentation() => "{}";
}