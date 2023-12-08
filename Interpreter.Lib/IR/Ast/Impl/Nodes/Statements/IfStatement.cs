using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Interpreter.Lib.IR.CheckSemantics.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

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
    }

    public bool Empty() =>
        !Then.Any() && !HasElseBlock();

    public bool HasElseBlock() =>
        Else is not null && Else.Any();

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Test;
        yield return Then;
        if (Else is not null)
        {
            yield return Else;
        }
    }

    protected override string NodeRepresentation() => "if";

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        visitor.Visit(this);

    public override Type Accept(SemanticChecker visitor) =>
        visitor.Visit(this);
}