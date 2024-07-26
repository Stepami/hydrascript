using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Instructions;
using HydraScript.Lib.BackEnd.Instructions.WithAssignment;
using HydraScript.Lib.BackEnd.Instructions.WithJump;
using HydraScript.Lib.BackEnd.Values;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;

namespace HydraScript.Lib.IR.Ast.Visitors;

public class InstructionProvider : VisitorBase<IAbstractSyntaxTreeNode, AddressedInstructions>,
    IVisitor<ScriptBody, AddressedInstructions>,
    IVisitor<LexicalDeclaration, AddressedInstructions>,
    IVisitor<BlockStatement, AddressedInstructions>,
    IVisitor<InsideStatementJump, AddressedInstructions>,
    IVisitor<ExpressionStatement, AddressedInstructions>,
    IVisitor<ReturnStatement, AddressedInstructions>,
    IVisitor<FunctionDeclaration, AddressedInstructions>,
    IVisitor<WhileStatement, AddressedInstructions>,
    IVisitor<IfStatement, AddressedInstructions>
{
    private readonly IVisitor<IAbstractSyntaxTreeNode, AddressedInstructions> _expressionVisitor;

    public InstructionProvider()
    {
        _expressionVisitor = new ExpressionInstructionProvider();
    }

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

    public override AddressedInstructions DefaultVisit { get; } = [];

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
            InsideStatementJump.Break => InsideStatementJumpType.Break,
            InsideStatementJump.Continue => InsideStatementJumpType.Continue,
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
                return [new Return(primary.ToValue())];
        }

        var result = visitable.Expression.Accept(_expressionVisitor);
        var last = new Name(result.OfType<Simple>().Last().Left!);
        result.Add(new Return(last));

        return result;
    }

    public AddressedInstructions Visit(FunctionDeclaration visitable)
    {
        if (!visitable.Statements.Any())
            return [];

        var functionInfo = new FunctionInfo(visitable.Name);

        var result = new AddressedInstructions
        {
            new Goto(functionInfo.End),
            {
                new BeginBlock(BlockType.Function, blockId: functionInfo.ToString()),
                functionInfo.Start.Name
            }
        };

        result.AddRange(visitable.Statements.Accept(This));
        if (!visitable.HasReturnStatement())
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
            result.Add(new IfNotGoto(primary.ToValue(), endBlockLabel));
        else
        {
            result.AddRange(visitable.Condition.Accept(_expressionVisitor));
            var last = new Name(result.OfType<Simple>().Last().Left!);
            result.Add(new IfNotGoto(last, endBlockLabel));
        }

        result.AddRange(visitable.Statement.Accept(This));
        result.OfType<Goto>().Where(g => g.JumpType is not null)
            .ToList().ForEach(g =>
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
            });
        result.Add(new Goto(startBlockLabel));

        result.Add(new EndBlock(BlockType.Loop, blockId), endBlockLabel.Name);

        return result;
    }

    public AddressedInstructions Visit(IfStatement visitable)
    {
        if (visitable.Empty())
            return [];

        var blockId = $"if_else_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");

        var result = new AddressedInstructions();

        if (visitable.Test is PrimaryExpression primary)
            result.Add(new IfNotGoto(primary.ToValue(), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(_expressionVisitor));
            var last = new Name(result.OfType<Simple>().Last().Left!);
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

        result.OfType<Goto>().Where(g => g.JumpType is InsideStatementJumpType.Break)
            .ToList().ForEach(g => g.SetJump(endBlockLabel));

        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        return result;
    }
}