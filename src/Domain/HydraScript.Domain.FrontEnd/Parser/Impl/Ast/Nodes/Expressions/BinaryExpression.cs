namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class BinaryExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Left, Right];

    public Expression Left { get; }
    public string Operator { get; }
    public Expression Right { get; }

    public BinaryExpression(Expression left, string @operator, Expression right)
    {
        Left = left;
        Left.Parent = this;

        Operator = @operator;

        Right = right;
        Right.Parent = this;
    }

    protected override string NodeRepresentation() => Operator;
}