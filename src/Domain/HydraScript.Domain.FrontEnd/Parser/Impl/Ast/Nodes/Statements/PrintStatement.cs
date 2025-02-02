namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class PrintStatement : Statement
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children => [Expression];

    public Expression Expression { get; }

    public PrintStatement(Expression expression)
    {
        Expression = expression;
        Expression.Parent = this;
    }

    protected override string NodeRepresentation() => "print";
}