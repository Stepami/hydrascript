namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<AbstractSyntaxTreeNode>]
public partial class ArrayLiteral : ComplexLiteral
{
    public List<Expression> Expressions { get; }

    public ArrayLiteral(IEnumerable<Expression> expressions)
    {
        Expressions = new List<Expression>(expressions);
        Expressions.ForEach(expr => expr.Parent = this);
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator() =>
        Expressions.GetEnumerator();

    protected override string NodeRepresentation() => "[]";
}