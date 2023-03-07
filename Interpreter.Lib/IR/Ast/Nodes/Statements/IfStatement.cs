using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class IfStatement : Statement
{
    public Expression Test { get; }
    public Statement Then { get; }
    public Statement Else { get; }

    public IfStatement(Expression test, Statement then, Statement @else = null)
    {
        Test = test;
        Test.Parent = this;

        Then = then;
        Then.Parent = this;

        if (@else is not null)
        {
            Else = @else;
            Else.Parent = this;
        }

        CanEvaluate = true;
    }

    public bool Empty() =>
        !Then.Any() && (Else is null || !Else.Any());

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Test;
        yield return Then;
        if (Else is not null)
        {
            yield return Else;
        }
    }

    internal override Type NodeCheck()
    {
        var testType = Test.NodeCheck();
        if (!testType.Equals(TypeUtils.JavaScriptTypes.Boolean))
        {
            throw new NotBooleanTestExpression(Segment, testType);
        }

        return testType;
    }

    protected override string NodeRepresentation() => "if";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);
}