using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

public class ReturnStatement : Statement
{
    public Expression Expression { get; }

    public ReturnStatement(Expression expression = null)
    {
        Expression = expression;
        if (expression is not null)
        {
            Expression.Parent = this;
        }
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        if (Expression is null)
        {
            yield break;
        }

        yield return Expression;
    }

    protected override string NodeRepresentation() => "return";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);
}