namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ArrayLiteral : ComplexLiteral
{
    private readonly List<Expression> _expressions;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _expressions;

    public IReadOnlyList<Expression> Expressions => _expressions;

    public ArrayLiteral(IEnumerable<Expression> expressions)
    {
        _expressions = new List<Expression>(expressions);
        _expressions.ForEach(expr => expr.Parent = this);
    }

    protected override string NodeRepresentation() => "[]";
}