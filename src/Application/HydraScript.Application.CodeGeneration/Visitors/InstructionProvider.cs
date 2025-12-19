using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment.ExplicitCast;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Domain.Constants;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Application.CodeGeneration.Visitors;

internal class InstructionProvider : VisitorBase<IAbstractSyntaxTreeNode, AddressedInstructions>,
    IVisitor<ScriptBody, AddressedInstructions>,
    IVisitor<LexicalDeclaration, AddressedInstructions>,
    IVisitor<BlockStatement, AddressedInstructions>,
    IVisitor<InsideStatementJump, AddressedInstructions>,
    IVisitor<ExpressionStatement, AddressedInstructions>,
    IVisitor<ReturnStatement, AddressedInstructions>,
    IVisitor<FunctionDeclaration, AddressedInstructions>,
    IVisitor<WhileStatement, AddressedInstructions>,
    IVisitor<IfStatement, AddressedInstructions>,
    IVisitor<OutputStatement, AddressedInstructions>
{
    private readonly IValueFactory _valueFactory;
    private readonly IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> _expressionVisitor;

    public InstructionProvider(
        IValueFactory valueFactory,
        [FromKeyedServices(CodeGeneratorType.Expression)]
        IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> expressionVisitor)
    {
        _valueFactory = valueFactory;
        _expressionVisitor = expressionVisitor;
    }

    public override AddressedInstructions Visit(IAbstractSyntaxTreeNode visitable) => [];

    public AddressedInstructions Visit(ScriptBody visitable)
    {
        var result = new AddressedInstructions();
        for (var i = 0; i < visitable.Count; i++)
        {
            var instructions = visitable[i].Accept(This);
            result.AddRange(instructions);
        }

        result.Add(new Halt());

        return result;
    }

    public AddressedInstructions Visit(LexicalDeclaration visitable)
    {
        var result = new AddressedInstructions();
        for (var i = 0; i < visitable.Count; i++)
        {
            var assignment = visitable[i];
            result.AddRange(assignment.Accept(_expressionVisitor));
        }

        return result;
    }

    public AddressedInstructions Visit(BlockStatement visitable)
    {
        var result = new AddressedInstructions();
        for (var i = 0; i < visitable.Count; i++)
        {
            var item = visitable[i];
            result.AddRange(item.Accept(This));
            if (item is ReturnStatement) break;
        }

        return result;
    }

    public AddressedInstructions Visit(InsideStatementJump visitable)
    {
        var jumpType = visitable.Keyword switch
        {
            InsideStatementJumpKeyword.Break => InsideStatementJumpType.Break,
            InsideStatementJumpKeyword.Continue => InsideStatementJumpType.Continue,
            _ => throw new ArgumentOutOfRangeException(
                nameof(visitable.Keyword), visitable.Keyword,
                "Unsupported keyword inside loop")
        };

        return [new Goto(jumpType)];
    }

    public AddressedInstructions Visit(ExpressionStatement visitable) =>
        visitable.Expression.Accept(_expressionVisitor);

    public AddressedInstructions Visit(ReturnStatement visitable)
    {
        switch (visitable.Expression)
        {
            case null:
                return [new Return()];
            case PrimaryExpression primary:
                return [new Return(_valueFactory.Create(primary.ToValueDto()))];
        }

        var result = visitable.Expression.Accept(_expressionVisitor);
        var last = result.OfType<Simple>().Last().Left!;
        result.Add(new Return(last));

        return result;
    }

    public AddressedInstructions Visit(FunctionDeclaration visitable)
    {
        if (visitable.IsEmpty)
            return [];

        var functionInfo = new FunctionInfo(visitable.ComputedFunctionAddress);

        var result = new AddressedInstructions
        {
            new Goto(functionInfo.End),
            {
                new BeginBlock(BlockType.Function, blockId: functionInfo.ToString()),
                functionInfo.Start.Name
            }
        };

        for (var i = 0; i < visitable.Arguments.Count; i++)
        {
            var arg = visitable.Arguments[i];
            result.Add(new PopParameter(_valueFactory.CreateName(arg.Name), arg.Info.Value));
        }

        result.AddRange(visitable.Statements.Accept(This));
        if (!visitable.HasReturnStatement)
            result.Add(new Return());

        result.Add(new EndBlock(BlockType.Function, blockId: functionInfo.ToString()), functionInfo.End.Name);

        return result;
    }

    public AddressedInstructions Visit(WhileStatement visitable)
    {
        var blockId = $"while_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");

        var result = new AddressedInstructions
        {
            { new BeginBlock(BlockType.Loop, blockId), startBlockLabel.Name }
        };

        if (visitable.Condition is PrimaryExpression primary)
            result.Add(new IfNotGoto(test: _valueFactory.Create(primary.ToValueDto()), endBlockLabel));
        else
        {
            result.AddRange(visitable.Condition.Accept(_expressionVisitor));
            var last = result.OfType<Simple>().Last().Left!;
            result.Add(new IfNotGoto(last, endBlockLabel));
        }

        result.AddRange(visitable.Statement.Accept(This));
        for (var address = result.Start; address != null; address = address.Next)
        {
            if (result[address] is Goto { JumpType: not null } g)
            {
                // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
                switch (g.JumpType)
                {
                    case InsideStatementJumpType.Break:
                        g.SetJump(endBlockLabel);
                        break;
                    case InsideStatementJumpType.Continue:
                        g.SetJump(startBlockLabel);
                        break;
                }
            }
        }
        result.Add(new Goto(startBlockLabel));

        result.Add(new EndBlock(BlockType.Loop, blockId), endBlockLabel.Name);

        return result;
    }

    public AddressedInstructions Visit(IfStatement visitable)
    {
        if (visitable.Empty)
            return [];

        var blockId = $"if_else_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");

        var result = new AddressedInstructions();

        if (visitable.Test is PrimaryExpression primary)
            result.Add(new IfNotGoto(test: _valueFactory.Create(primary.ToValueDto()), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(_expressionVisitor));
            var last = result.OfType<Simple>().Last().Left!;
            result.Add(new IfNotGoto(last,
                visitable.HasElseBlock()
                    ? startBlockLabel
                    : endBlockLabel)
            );
        }

        result.AddRange(visitable.Then.Accept(This));
        result.Add(new Goto(endBlockLabel));
        result.Add(new BeginBlock(BlockType.Condition, blockId), startBlockLabel.Name);

        if (visitable.HasElseBlock())
            result.AddRange(visitable.Else?.Accept(This) ?? []);

        for (var address = result.Start; address != null; address = address.Next)
        {
            if (result[address] is Goto { JumpType: InsideStatementJumpType.Break } g)
                g.SetJump(endBlockLabel);
        }

        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        return result;
    }

    public AddressedInstructions Visit(OutputStatement visitable)
    {
        if (visitable.Expression is PrimaryExpression prim)
        {
            var valueDto = prim.ToValueDto();
            var printedValue = _valueFactory.Create(valueDto);
            IExecutableInstruction instruction = valueDto is { Type: ValueDtoType.Env } or { Type: ValueDtoType.Constant, Value: string }
                ? new Print(printedValue)
                : new AsString(printedValue);
            AddressedInstructions shortResult = [instruction];
            if (instruction is AsString asString)
                shortResult.Add(new Print(asString.Left!));
            return shortResult;
        }

        AddressedInstructions result = [];

        result.AddRange(visitable.Expression.Accept(_expressionVisitor));
        var name = result.OfType<Simple>().Last().Left!;
        var nameAsString = new AsString(name);
        result.Add(nameAsString);
        result.Add(new Print(nameAsString.Left!));

        return result;
    }
}