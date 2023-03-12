using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;

public class Property : Expression
{
    public IdentifierReference Id { get; }
    public Expression Expression { get; }

    public ObjectLiteral Object =>
        Parent as ObjectLiteral;

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
   
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Id;
        yield return Expression;
    }

    protected override string NodeRepresentation() => ":";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}