using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public class ConditionalExpression : Expression
{
    public Expression Test { get; }
    public Expression Consequent { get; }
    public Expression Alternate { get; }

    public ConditionalExpression(Expression test, Expression consequent, Expression alternate)
    {
        Test = test;
        Consequent = consequent;
        Alternate = alternate;

        Test.Parent = this;
        Consequent.Parent = this;
        Alternate.Parent = this;
    }

    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield return Test;
        yield return Consequent;
        yield return Alternate;
    }

    protected override string NodeRepresentation() => "?:";

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}