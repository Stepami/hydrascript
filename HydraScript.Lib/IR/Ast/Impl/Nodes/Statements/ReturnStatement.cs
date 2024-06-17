using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;
using HydraScript.Lib.IR.CheckSemantics.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

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