using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.CheckSemantics.Variables;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class CastAsExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Expression];

    public Expression Expression { get; }
    public TypeValue Cast { get; }

    public CastAsExpression(Expression expression, TypeValue cast)
    {
        Expression = expression;
        Expression.Parent = this;

        Cast = cast;
    }

    /// <inheritdoc cref="AbstractSyntaxTreeNode.InitScope"/>
    public override void InitScope(SymbolTable? scope = null)
    {
        base.InitScope(scope);
        Cast.SymbolTable = SymbolTable;
    }

    protected override string NodeRepresentation() => $"as {Cast}";
}