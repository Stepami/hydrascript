using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class ExpressionInstructionProvider :
    IVisitor<PrimaryExpression, AddressedInstructions>,
    IVisitor<UnaryExpression, AddressedInstructions>,
    IVisitor<BinaryExpression, AddressedInstructions>,
    IVisitor<CastAsExpression, AddressedInstructions>
{
    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        new() { new Simple(visitable.ToValue()) };

    public AddressedInstructions Visit(UnaryExpression visitable)
    {
        var result = visitable.Expression.Accept(this);
        var last = new Name(result.OfType<Simple>().Last().Left);
        result.Add(new Simple(visitable.Operator, last));
        
        return result;
    }

    public AddressedInstructions Visit(BinaryExpression visitable)
    {
        if (visitable.Left is IdentifierReference arr &&
            visitable.Right is PrimaryExpression primary &&
            visitable.Operator == "::")
            return new AddressedInstructions
            {
                new RemoveFromArray(arr.Id, primary.ToValue())
            };

        var result = new AddressedInstructions();
        
        result.AddRange(visitable.Left.Accept(this));
        var left = new Name(result.OfType<Simple>().Last().Left);
        
        result.AddRange(visitable.Right.Accept(this));
        var right = new Name(result.OfType<Simple>().Last().Left);
        
        result.Add(new Simple(left, visitable.Operator, right));

        return result;
    }

    public AddressedInstructions Visit(CastAsExpression visitable)
    {
        var result = visitable.Expression.Accept(this);
        var last = new Name(result.OfType<Simple>().Last().Left);
        result.Add(new AsString(last));
        
        return result;
    }
}