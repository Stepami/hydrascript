namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ObjectLiteral : ComplexLiteral
{
    private readonly List<Property> _properties;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _properties;

    public IReadOnlyList<Property> Properties => _properties;

    public ObjectLiteral(IEnumerable<Property> properties)
    {
        _properties = new List<Property>(properties);
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