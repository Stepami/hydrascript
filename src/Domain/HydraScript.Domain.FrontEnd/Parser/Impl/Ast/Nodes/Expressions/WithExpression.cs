using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class WithExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children { get; }

    public Expression Expression { get; }
    public ObjectLiteral ObjectLiteral { get; }

    public WithExpression(Expression expression, ObjectLiteral objectLiteral)
    {
        Expression = expression;
        Expression.Parent = this;

        ObjectLiteral = objectLiteral;
        ObjectLiteral.Parent = this;

        Children = [Expression, ObjectLiteral];
    }

    protected override string NodeRepresentation() => "with";
}
