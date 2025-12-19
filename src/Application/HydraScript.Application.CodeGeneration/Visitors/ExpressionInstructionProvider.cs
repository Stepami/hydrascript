using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Create;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Read;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ComplexData.Write;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;
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
    IVisitor<WithExpression, AddressedInstructions>,
    IVisitor<ConditionalExpression, AddressedInstructions>,
    IVisitor<AssignmentExpression, AddressedInstructions>,
    IVisitor<MemberExpression, AddressedInstructions>,
    IVisitor<DotAccess, AddressedInstructions>,
    IVisitor<IndexAccess, AddressedInstructions>,
    IVisitor<CallExpression, AddressedInstructions>
{
    private readonly IValueFactory _valueFactory;

    public ExpressionInstructionProvider(IValueFactory valueFactory) =>
        _valueFactory = valueFactory;

    public override AddressedInstructions Visit(IAbstractSyntaxTreeNode visitable) => [];

    public AddressedInstructions Visit(PrimaryExpression visitable) =>
        [new Simple(_valueFactory.Create(visitable.ToValueDto()))];

    public AddressedInstructions Visit(ArrayLiteral visitable)
    {
        var arraySize = visitable.Expressions.Count;

        var arrayName = _valueFactory.CreateName(visitable.Id);
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
                    _valueFactory.Create(primary.ToValueDto())));
            else
            {
                result.AddRange(expression.Accept(This));
                var last = result.OfType<Simple>().Last().Left!;
                result.Add(new IndexAssignment(arrayName, index, last));
            }
        }

        return result;
    }

    public AddressedInstructions Visit(ObjectLiteral visitable)
    {
        var objectId = _valueFactory.CreateName(visitable.Id);
        var createObject = new CreateObject(objectId);

        var result = new AddressedInstructions { createObject };

        var propInstructions = visitable.AsValueEnumerable()
            .SelectMany(property => property.Accept(This))
            .ToList();
        result.AddRange(propInstructions);

        return result;
    }

    public AddressedInstructions Visit(Property visitable)
    {
        var objectId = _valueFactory.CreateName(visitable.Object.Id);

        var (id, expression) = visitable;
        var propertyId = new Constant(id);

        if (expression is PrimaryExpression primary)
            return [new DotAssignment(
                objectId,
                propertyId,
                _valueFactory.Create(primary.ToValueDto()))];

        var instructions = expression.Accept(This);
        var last = instructions.OfType<Simple>().Last().Left!;
        instructions.Add(new DotAssignment(objectId, propertyId, last));

        return instructions;
    }

    public AddressedInstructions Visit(UnaryExpression visitable)
    {
        if (visitable.Expression is PrimaryExpression primary)
            return [new Simple(visitable.Operator, _valueFactory.Create(primary.ToValueDto()))];
        
        var result = visitable.Expression.Accept(This);
        var last = result.OfType<Simple>().Last().Left!;
        result.Add(new Simple(visitable.Operator, last));
        
        return result;
    }

    public AddressedInstructions Visit(BinaryExpression visitable)
    {
        if (visitable is { Left: IdentifierReference arr, Right: PrimaryExpression primary, Operator: "::" })
            return [new RemoveFromArray(_valueFactory.CreateName(arr), index: _valueFactory.Create(primary.ToValueDto()))];

        var result = new AddressedInstructions();
        IValue left, right;

        if (visitable.Left is PrimaryExpression primaryLeft)
            left = _valueFactory.Create(primaryLeft.ToValueDto());
        else
        {
            result.AddRange(visitable.Left.Accept(This));
            left = result.OfType<Simple>().Last().Left!;
        }

        if (visitable.Right is PrimaryExpression primaryRight)
            right = _valueFactory.Create(primaryRight.ToValueDto());
        else
        {
            result.AddRange(visitable.Right.Accept(This));
            right = result.OfType<Simple>().Last().Left!;
        }

        result.Add(new Simple(left, visitable.Operator, right));

        return result;
    }

    public AddressedInstructions Visit(CastAsExpression visitable)
    {
        Func<IValue, IExecutableInstruction> asFactory = visitable.ToType switch
        {
            CastAsExpression.DestinationType.Undefined => throw new NotSupportedException(),
            CastAsExpression.DestinationType.String => value => new AsString(value),
            CastAsExpression.DestinationType.Number => value => new AsNumber(value),
            CastAsExpression.DestinationType.Boolean => value => new AsBool(value),
            _ => throw new ArgumentOutOfRangeException(nameof(visitable.ToType))
        };

        if (visitable.Expression is PrimaryExpression primary)
            return [asFactory(_valueFactory.Create(primary.ToValueDto()))];

        var result = visitable.Expression.Accept(This);
        var last = result.OfType<Simple>().Last().Left!;
        result.Add(asFactory(last));

        return result;
    }

    public AddressedInstructions Visit(WithExpression visitable)
    {
        var objectId = _valueFactory.CreateName(visitable.ObjectLiteral.Id);
        var createObject = new CreateObject(objectId);

        var result = new AddressedInstructions { createObject };

        if (visitable is { Expression: ObjectLiteral left, ObjectLiteral: {} right })
        {
            result.AddRange(left.AsValueEnumerable().Concat(right)
                .SelectMany(property => property.Accept(This))
                .ToList());
            return result;
        }

        var propInstructions = visitable.ObjectLiteral
            .AsValueEnumerable()
            .SelectMany(property => property.Accept(This))
            .ToList();
        result.AddRange(propInstructions);

        if (visitable.ComputedCopiedProperties.Count is 0)
            return result;

        result.AddRange(visitable.Expression is PrimaryExpression ? [] : visitable.Expression.Accept(This));

        var copyFrom = visitable.Expression is IdentifierReference objectIdent
            ? _valueFactory.CreateName(objectIdent)
            : result.OfType<Simple>().Last().Left!;

        for (var i = 0; i < visitable.ComputedCopiedProperties.Count; i++)
        {
            var property = new Constant(visitable.ComputedCopiedProperties[i]);
            result.Add(new DotRead(copyFrom, property));
            var read = result[result.End].Address.Name;
            result.Add(new DotAssignment(objectId, property, _valueFactory.CreateName(read)));
        }

        return result;
    }

    public AddressedInstructions Visit(ConditionalExpression visitable)
    {
        var blockId = $"cond_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");
        
        var result = new AddressedInstructions();

        if (visitable.Test is PrimaryExpression primary)
            result.Add(new IfNotGoto(test: _valueFactory.Create(primary.ToValueDto()), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(This));
            var last = result.OfType<Simple>().Last().Left!;
            result.Add(new IfNotGoto(last, startBlockLabel));
        }

        result.AddRange(visitable.Consequent.Accept(This));
        var temp = result.OfType<Simple>().Last().Left!;
        result.Add(new Goto(endBlockLabel));
        
        result.Add(new BeginBlock(BlockType.Condition, blockId), startBlockLabel.Name);
        result.AddRange(visitable.Alternate.Accept(This));
        result.OfType<Simple>().Last().Left = temp;
        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        result.Add(new Simple(temp));

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
                result.Add(new Simple(last.Left!));
        }

        if (visitable.Destination.Empty())
            result.OfType<Simple>().Last().Left = _valueFactory.CreateName(visitable.Destination.Id);
        else
        {
            var last = result.OfType<Simple>().Last().Left!;
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
            return [new DotRead(_valueFactory.CreateName(lhs.Id), right)];

        var result = visitable.Prev?.Accept(This) ?? [];
        var left = result.OfType<Simple>().Last().Left!;
        result.Add(new DotRead(left, right));

        return result;
    }

    public AddressedInstructions Visit(IndexAccess visitable)
    {
        var result = new AddressedInstructions();
        
        IValue right;

        if (visitable.Index is PrimaryExpression primary)
            right = _valueFactory.Create(primary.ToValueDto());
        else
        {
            result.AddRange(visitable.Index.Accept(This));
            right = result.OfType<Simple>().Last().Left!;
        }

        if (!visitable.HasPrev() && visitable.Parent is LeftHandSideExpression lhs)
            result.Add(new IndexRead(_valueFactory.CreateName(lhs.Id), right));
        else
        {
            result.AddRange(visitable.Prev?.Accept(This) ?? []);
            var left = result.OfType<Simple>().Last().Left!;
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
            var caller = result.Count > 0
                ? result.OfType<Simple>().Last().Left!
                : _valueFactory.CreateName(visitable.Id);
            result.Add(new PushParameter(caller));
        }

        for (var i = 0; i < visitable.Parameters.Count; i++)
        {
            var expr = visitable.Parameters[i];
            if (expr is PrimaryExpression primary)
                result.Add(new PushParameter(_valueFactory.Create(primary.ToValueDto())));
            else
            {
                result.AddRange(expr.Accept(This));
                var id = result.OfType<Simple>().Last().Left!;
                result.Add(new PushParameter(id));
            }
        }

        result.Add(new CallFunction(
            new FunctionInfo(visitable.ComputedFunctionAddress),
            visitable.HasReturnValue));

        return result;
    }
}