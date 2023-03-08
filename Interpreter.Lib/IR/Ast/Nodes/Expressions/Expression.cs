using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast.Visitors;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Nodes.Expressions;

public abstract class Expression : AbstractSyntaxTreeNode,
    IVisitable<ExpressionInstructionProvider, AddressedInstructions>
{
    protected Expression()
    {
        CanEvaluate = true;
    }

    public abstract AddressedInstructions Accept(ExpressionInstructionProvider visitor);

    public override AddressedInstructions Accept(InstructionProvider visitor) =>
        throw new NotImplementedException();
}