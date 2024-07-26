using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class CallExpression : LeftHandSideExpression
{
    private readonly List<Expression> _parameters;

    protected override IReadOnlyList<AbstractSyntaxTreeNode> Children =>
        [Member, .._parameters];

    public MemberExpression Member { get; }
    public IReadOnlyList<Expression> Parameters => _parameters;

    public CallExpression(MemberExpression member, IEnumerable<Expression> expressions)
    {
        Member = member;
        Member.Parent = this;

        _parameters = new List<Expression>(expressions);
        _parameters.ForEach(expr => expr.Parent = this);
    }

    public override IdentifierReference Id => Member.Id;

    public override bool Empty() => Member.Empty();

    protected override string NodeRepresentation() => "()";
}