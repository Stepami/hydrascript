using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class ExpressionInstructionProvider :
    IVisitor<PrimaryExpression, AddressedInstructions>
{
    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        new() { new Simple(visitable.ToValue()) };
}