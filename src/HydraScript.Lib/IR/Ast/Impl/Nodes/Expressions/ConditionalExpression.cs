namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ConditionalExpression : Expression
{
    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Test, Consequent, Alternate];

    public Expression Test { get; }
    public Expression Consequent { get; }
    public Expression Alternate { get; }

    public ConditionalExpression(Expression test, Expression consequent, Expression alternate)
    {
        Test = test;
        Consequent = consequent;
        Alternate = alternate;

        Test.Parent = this;
        Consequent.Parent = this;
        Alternate.Parent = this;
    }

    protected override string NodeRepresentation() => "?:";
}