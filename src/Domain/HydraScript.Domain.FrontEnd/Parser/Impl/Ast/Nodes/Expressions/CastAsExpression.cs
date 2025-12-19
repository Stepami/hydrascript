using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class CastAsExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public Expression Expression { get; }
    public TypeValue Cast { get; }
    public DestinationType ToType { get; set; }

    public CastAsExpression(Expression expression, TypeValue cast)
    {
        Expression = expression;
        Expression.Parent = this;

        Cast = cast;

        Children = [Expression];
    }

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(Scope? scope = null)
    {
        base.InitScope(scope);
        Cast.Scope = Scope;
    }

    protected override string NodeRepresentation() => $"as {Cast}";

    public enum DestinationType
    {
        Undefined,
        String,
        Number,
        Boolean,
    }
}