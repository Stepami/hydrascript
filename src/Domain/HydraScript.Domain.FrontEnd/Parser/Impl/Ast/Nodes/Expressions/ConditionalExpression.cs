namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ConditionalExpression : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
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