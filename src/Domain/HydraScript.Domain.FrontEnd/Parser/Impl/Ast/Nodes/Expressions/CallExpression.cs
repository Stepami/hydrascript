using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class CallExpression : LeftHandSideExpression
{
    private readonly List<Expression> _parameters;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Member, .._parameters];

    public MemberExpression Member { get; }
    public IReadOnlyList<Expression> Parameters => _parameters;

    public bool IsEmptyCall { get; set; }
    public bool HasReturnValue { get; set; }

    public string ComputedFunctionAddress { get; set; } = default!;

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