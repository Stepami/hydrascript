namespace HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class ArrayLiteral : ComplexLiteral
{
    private readonly List<Expression> _expressions;

    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        _expressions;

    public IReadOnlyList<Expression> Expressions => _expressions;

    public ArrayLiteral(List<Expression> expressions)
    {
        _expressions = expressions;
        _expressions.ForEach(expr => expr.Parent = this);
    }

    protected override string NodeRepresentation() => "[]";
}