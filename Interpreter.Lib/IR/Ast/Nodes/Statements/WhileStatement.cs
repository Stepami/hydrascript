using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class WhileStatement : Statement
{
    public Expression Condition { get; }
    public Statement Statement { get; }

    public WhileStatement(Expression condition, Statement statement)
    {
        Condition = condition;
        Condition.Parent = this;

        Statement = statement;
        Statement.Parent = this;

        CanEvaluate = true;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Condition;
        yield return Statement;
    }

    internal override Type NodeCheck()
    {
        var condType = Condition.NodeCheck();
        if (!condType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            throw new NotBooleanTestExpression(Segment, condType);
        }

        return condType;
    }

    protected override string NodeRepresentation() => "while";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}