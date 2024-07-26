using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class CastAsExpression : Expression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Expression];

    public Expression Expression { get; }
    public TypeValue Cast { get; }

    public CastAsExpression(Expression expression, TypeValue cast)
    {
        Expression = expression;
        Expression.Parent = this;

        Cast = cast;
    }

    protected override string NodeRepresentation() => $"as {Cast}";
}