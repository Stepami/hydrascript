using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;

public abstract class PrimaryExpression : Expression
{
    public override IEnumerator<AbstractSyntaxTreeNode> GetEnumerator()
    {
        yield break;
    }

    public abstract IValue ToValue();

    public override AddressedInstructions Accept(ExpressionInstructionProvider visitor) =>
        visitor.Visit(this);
}