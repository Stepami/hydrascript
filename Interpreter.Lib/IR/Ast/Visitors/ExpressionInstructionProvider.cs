using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class ExpressionInstructionProvider :
    IVisitor<PrimaryExpression, AddressedInstructions>,
    IVisitor<ArrayLiteral, AddressedInstructions>,
    IVisitor<Property, AddressedInstructions>,
    IVisitor<UnaryExpression, AddressedInstructions>,
    IVisitor<BinaryExpression, AddressedInstructions>,
    IVisitor<CastAsExpression, AddressedInstructions>,
    IVisitor<ConditionalExpression, AddressedInstructions>,
    IVisitor<AssignmentExpression, AddressedInstructions>
{
    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        new() { new Simple(visitable.ToValue()) };

    public AddressedInstructions Visit(ArrayLiteral visitable)
    {
        var arraySize = visitable.Expressions.Count;

        var arrayName = visitable.Id;
        var createArray = new CreateArray(arrayName, arraySize);

        var result = new AddressedInstructions { createArray };

        for (var i = 0; i < arraySize; i++)
        {
            var expression = visitable.Expressions[i];
            var index = new Constant(i);
            
            if (expression is PrimaryExpression primary)
                result.Add(new IndexAssignment(arrayName, index, primary.ToValue()));
            else
            {
                result.AddRange(expression.Accept(this));
                var last = new Name(result.OfType<Simple>().Last().Left);
                result.Add(new IndexAssignment(arrayName, index, last));
            }
        }

        return result;
    }
    
    public AddressedInstructions Visit(Property visitable)
    {
        var objectId = visitable.Object.Id;

        var (id, expression) = visitable;
        var propertyId = new Constant(id, @$"\""{id}\""");

        if (expression is PrimaryExpression primary)
            return new AddressedInstructions
                { new DotAssignment(objectId, propertyId, primary.ToValue()) };

        var instructions = expression.Accept(this);
        var last = new Name(instructions.OfType<Simple>().Last().Left);
        instructions.Add(new DotAssignment(objectId, propertyId, last));

        return instructions;
    }

    public AddressedInstructions Visit(UnaryExpression visitable)
    {
        if (visitable.Expression is PrimaryExpression primary)
            return new() { new Simple(visitable.Operator, primary.ToValue()) };
        
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
        IValue left, right;

        if (visitable.Left is PrimaryExpression primaryLeft)
            left = primaryLeft.ToValue();
        else
        {
            result.AddRange(visitable.Left.Accept(this));
            left = new Name(result.OfType<Simple>().Last().Left);
        }

        if (visitable.Right is PrimaryExpression primaryRight)
            right = primaryRight.ToValue();
        else
        {
            result.AddRange(visitable.Right.Accept(this));
            right = new Name(result.OfType<Simple>().Last().Left);
        }

        result.Add(new Simple(left, visitable.Operator, right));

        return result;
    }

    public AddressedInstructions Visit(CastAsExpression visitable)
    {
        if (visitable.Expression is PrimaryExpression primary)
            return new() { new AsString(primary.ToValue()) };
        
        var result = visitable.Expression.Accept(this);
        var last = new Name(result.OfType<Simple>().Last().Left);
        result.Add(new AsString(last));
        
        return result;
    }

    public AddressedInstructions Visit(ConditionalExpression visitable)
    {
        var blockId = $"cond_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");
        
        var result = new AddressedInstructions();

        if (visitable.Test is PrimaryExpression primary)
            result.Add(new IfNotGoto(primary.ToValue(), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(this));
            var last = new Name(result.OfType<Simple>().Last().Left);
            result.Add(new IfNotGoto(last, startBlockLabel));
        }

        result.AddRange(visitable.Consequent.Accept(this));
        var temp = result.OfType<Simple>().Last().Left;
        result.Add(new Goto(endBlockLabel));
        
        result.Add(new BeginBlock(BlockType.Condition, blockId), startBlockLabel.Name);
        result.AddRange(visitable.Alternate.Accept(this));
        result.OfType<Simple>().Last().Left = temp;
        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        result.Add(new Simple(new Name(temp)));

        return result;
    }

    public AddressedInstructions Visit(AssignmentExpression visitable)
    {
        var result = visitable.Source.Accept(this);
        result.OfType<Simple>().Last().Left = visitable.Destination.Id;
        return result;
    }
}