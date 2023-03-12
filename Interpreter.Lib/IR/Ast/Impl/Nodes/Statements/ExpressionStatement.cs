using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

public class ExpressionStatement : Statement
{
    public Expression Expression { get; }

    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
        expression.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Expression;
    }

    protected override string NodeRepresentation() => nameof(ExpressionStatement);

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}