using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

[AutoVisitable<IAbstractSyntaxTreeNode>]
public partial class Property : Expression
{
    protected override IReadOnlyList<IAbstractSyntaxTreeNode> Children =>
        [Id, Expression];

    public IdentifierReference Id { get; }
    public Expression Expression { get; }

    public ObjectLiteral Object =>
        (Parent as ObjectLiteral)!;

    public Property(IdentifierReference id, Expression expression)
    {
        Id = id;
        Id.Parent = this;

        Expression = expression;
        Expression.Parent = this;
    }

    public void Deconstruct(out string id, out Expression expr)
    {
        id = Id.Name;
        expr = Expression;
    }

    protected override string NodeRepresentation() => ":";
}