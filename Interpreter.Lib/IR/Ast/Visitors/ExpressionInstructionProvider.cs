using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class ExpressionInstructionProvider :
    IVisitor<PrimaryExpression, AddressedInstructions>,
    IVisitor<UnaryExpression, AddressedInstructions>
{
    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        new() { new Simple(visitable.ToValue()) };

    public AddressedInstructions Visit(UnaryExpression visitable)
    {
        var instructions = visitable.Expression.Accept(this);
        var last = instructions.OfType<Simple>().Last();
        instructions.Add(new Simple(visitable.Operator, new Name(last.Left)));
        
        return instructions;
    }
}