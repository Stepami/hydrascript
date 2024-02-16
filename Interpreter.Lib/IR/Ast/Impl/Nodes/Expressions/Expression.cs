using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;

namespace Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;

public abstract class Expression : AbstractSyntaxTreeNode,
    IVisitable<ExpressionInstructionProvider, AddressedInstructions>
{
    protected Expression()
    {
    }

    public abstract AddressedInstructions Accept(ExpressionInstructionProvider visitor);

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        throw new NotSupportedException();
}