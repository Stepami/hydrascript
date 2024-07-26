namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class BinaryExpression : Expression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
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