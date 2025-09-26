using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.BackEnd.Impl.Values;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using ZLinq;

namespace HydraScript.Application.CodeGeneration.Visitors;

internal class ExpressionInstructionProvider : VisitorBase<IAbstractSyntaxTreeNode, AddressedInstructions>,
    IVisitor<PrimaryExpression, AddressedInstructions>,
    IVisitor<ArrayLiteral, AddressedInstructions>,
    IVisitor<ObjectLiteral, AddressedInstructions>,
    IVisitor<Property, AddressedInstructions>,
    IVisitor<UnaryExpression, AddressedInstructions>,
    IVisitor<BinaryExpression, AddressedInstructions>,
    IVisitor<CastAsExpression, AddressedInstructions>,
    IVisitor<ConditionalExpression, AddressedInstructions>,
    IVisitor<AssignmentExpression, AddressedInstructions>,
    IVisitor<MemberExpression, AddressedInstructions>,
    IVisitor<DotAccess, AddressedInstructions>,
    IVisitor<IndexAccess, AddressedInstructions>,
    IVisitor<CallExpression, AddressedInstructions>
{
    private readonly IValueDtoConverter _valueDtoConverter;

    public ExpressionInstructionProvider(IValueDtoConverter valueDtoConverter) =>
        _valueDtoConverter = valueDtoConverter;

    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        [new Simple(_valueDtoConverter.Convert(visitable.ToValueDto()))];

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
                result.Add(new IndexAssignment(
                    arrayName,
                    index,
                    _valueDtoConverter.Convert(primary.ToValueDto())));
            else
            {
                result.AddRange(expression.Accept(This));
                var last = new Name(result.OfType<Simple>().Last().Left!);
                result.Add(new IndexAssignment(arrayName, index, last));
            }
        }

        return result;
    }

    public AddressedInstructions Visit(ObjectLiteral visitable)
    {
        var objectId = visitable.Id;
        var createObject = new CreateObject(objectId);

        var result = new AddressedInstructions { createObject };

        var propInstructions =
            visitable.Properties.AsValueEnumerable()
                .SelectMany(property => property.Accept(This));
        foreach (var propInstruction in propInstructions)
            result.Add(propInstruction);

        return result;
    }

    public AddressedInstructions Visit(Property visitable)
    {
        var objectId = visitable.Object.Id;

        var (id, expression) = visitable;
        var propertyId = new Constant(id);

        if (expression is PrimaryExpression primary)
            return [new DotAssignment(
                objectId,
                propertyId,
                _valueDtoConverter.Convert(primary.ToValueDto()))];

        var instructions = expression.Accept(This);
        var last = new Name(instructions.OfType<Simple>().Last().Left!);
        instructions.Add(new DotAssignment(objectId, propertyId, last));

        return instructions;
    }

    public AddressedInstructions Visit(UnaryExpression visitable)
    {
        if (visitable.Expression is PrimaryExpression primary)
            return [new Simple(visitable.Operator, _valueDtoConverter.Convert(primary.ToValueDto()))];
        
        var result = visitable.Expression.Accept(This);
        var last = new Name(result.OfType<Simple>().Last().Left!);
        result.Add(new Simple(visitable.Operator, last));
        
        return result;
    }

    public AddressedInstructions Visit(BinaryExpression visitable)
    {
        if (visitable is { Left: IdentifierReference arr, Right: PrimaryExpression primary, Operator: "::" })
            return [new RemoveFromArray(arr.Name, index: _valueDtoConverter.Convert(primary.ToValueDto()))];

        var result = new AddressedInstructions();
        IValue left, right;

        if (visitable.Left is PrimaryExpression primaryLeft)
            left = _valueDtoConverter.Convert(primaryLeft.ToValueDto());
        else
        {
            result.AddRange(visitable.Left.Accept(This));
            left = new Name(result.OfType<Simple>().Last().Left!);
        }

        if (visitable.Right is PrimaryExpression primaryRight)
            right = _valueDtoConverter.Convert(primaryRight.ToValueDto());
        else
        {
            result.AddRange(visitable.Right.Accept(This));
            right = new Name(result.OfType<Simple>().Last().Left!);
        }

        result.Add(new Simple(left, visitable.Operator, right));

        return result;
    }

    public AddressedInstructions Visit(CastAsExpression visitable)
    {
        if (visitable.Expression is PrimaryExpression primary)
            return [new AsString(_valueDtoConverter.Convert(primary.ToValueDto()))];
        
        var result = visitable.Expression.Accept(This);
        var last = new Name(result.OfType<Simple>().Last().Left!);
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
            result.Add(new IfNotGoto(test: _valueDtoConverter.Convert(primary.ToValueDto()), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(This));
            var last = new Name(result.OfType<Simple>().Last().Left!);
            result.Add(new IfNotGoto(last, startBlockLabel));
        }

        result.AddRange(visitable.Consequent.Accept(This));
        var temp = result.OfType<Simple>().Last().Left!;
        result.Add(new Goto(endBlockLabel));
        
        result.Add(new BeginBlock(BlockType.Condition, blockId), startBlockLabel.Name);
        result.AddRange(visitable.Alternate.Accept(This));
        result.OfType<Simple>().Last().Left = temp;
        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        result.Add(new Simple(new Name(temp)));

        return result;
    }

    public AddressedInstructions Visit(AssignmentExpression visitable)
    {
        var result = visitable.Source.Accept(This);
        if (visitable.Source is AssignmentExpression)
        {
            var last = result.OfType<Simple>().Last();
            if (last is IWriteToComplexData assignment)
                result.Add(assignment.ToSimple());
            else
                result.Add(new Simple(new Name(last.Left!)));
        }

        if (visitable.Destination.Empty())
            result.OfType<Simple>().Last().Left = visitable.Destination.Id;
        else
        {
            var last = new Name(result.OfType<Simple>().Last().Left!);
            result.AddRange(visitable.Destination.Accept(This));
            var lastRead = result.OfType<IReadFromComplexData>().Last();
            result.Replace(lastRead.ToInstruction(), lastRead.ToAssignment(last));
        }

        return result;
    }

    public AddressedInstructions Visit(MemberExpression visitable) =>
        visitable.Empty()
            ? []
            : visitable.Tail?.Accept(This) ?? [];

    public AddressedInstructions Visit(DotAccess visitable)
    {
        var right = new Constant(visitable.Property.Name);

        if (!visitable.HasPrev() && visitable.Parent is LeftHandSideExpression lhs)
            return [new DotRead(new Name(lhs.Id), right)];

        var result = visitable.Prev?.Accept(This) ?? [];
        var left = new Name(result.OfType<Simple>().Last().Left!);
        result.Add(new DotRead(left, right));

        return result;
    }

    public AddressedInstructions Visit(IndexAccess visitable)
    {
        var result = new AddressedInstructions();
        
        IValue right;

        if (visitable.Index is PrimaryExpression primary)
            right = _valueDtoConverter.Convert(primary.ToValueDto());
        else
        {
            result.AddRange(visitable.Index.Accept(This));
            right = new Name(result.OfType<Simple>().Last().Left!);
        }

        if (!visitable.HasPrev() && visitable.Parent is LeftHandSideExpression lhs)
            result.Add(new IndexRead(new Name(lhs.Id), right));
        else
        {
            result.AddRange(visitable.Prev?.Accept(This) ?? []);
            var left = new Name(result.OfType<Simple>().Last().Left!);
            result.Add(new IndexRead(left, right));
        }
        
        return result;
    }

    public AddressedInstructions Visit(CallExpression visitable)
    {
        if (visitable.IsEmptyCall)
            return [];

        var methodCall = !visitable.Empty();
        AddressedInstructions result = [];

        if (methodCall)
        {
            var memberInstructions = visitable.Member.Accept(This);
            var lastMemberInstruction = (DotRead)memberInstructions[memberInstructions.End];
            memberInstructions.Remove(lastMemberInstruction);
            result.AddRange(memberInstructions);
        }

        if (methodCall)
        {
            var caller = result.Any() ? result.OfType<Simple>().Last().Left! : visitable.Id;
            result.Add(new PushParameter(new Name(caller)));
        }

        for (var i = 0; i < visitable.Parameters.Count; i++)
        {
            var expr = visitable.Parameters[i];
            if (expr is PrimaryExpression primary)
                result.Add(new PushParameter(_valueDtoConverter.Convert(primary.ToValueDto())));
            else
            {
                result.AddRange(expr.Accept(This));
                var id = result.OfType<Simple>().Last().Left!;
                result.Add(new PushParameter(new Name(id)));
            }
        }

        result.Add(new CallFunction(
            new FunctionInfo(visitable.ComputedFunctionAddress),
            visitable.HasReturnValue));

        return result;
    }
}