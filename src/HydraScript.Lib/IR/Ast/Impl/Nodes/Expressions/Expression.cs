using HydraScript.Lib.BackEnd;
using HydraScript.Lib.IR.Ast.Visitors;

namespace HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;

public abstract class Expression : AbstractSyntaxTreeNode,
    IVisitable<ExpressionInstructionProvider, AddressedInstructions>
{
    public abstract AddressedInstructions Accept(ExpressionInstructionProvider visitor);

    public sealed override AddressedInstructions Accept(InstructionProvider visitor) =>
        throw new NotSupportedException();
}