using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment;
using Interpreter.Lib.BackEnd.Instructions.WithAssignment.ComplexData.Create;
using Interpreter.Lib.BackEnd.Instructions.WithJump;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class InstructionProvider :
    IVisitor<ScriptBody, AddressedInstructions>,
    IVisitor<LexicalDeclaration, AddressedInstructions>,
    IVisitor<BlockStatement, AddressedInstructions>,
    IVisitor<InsideStatementJump, AddressedInstructions>,
    IVisitor<ExpressionStatement, AddressedInstructions>,
    IVisitor<ReturnStatement, AddressedInstructions>,
    IVisitor<ObjectLiteral, AddressedInstructions>,
    IVisitor<FunctionDeclaration, AddressedInstructions>,
    IVisitor<WhileStatement, AddressedInstructions>,
    IVisitor<IfStatement, AddressedInstructions>
{
    private readonly ExpressionInstructionProvider _expressionVisitor = new();
    
    public AddressedInstructions Visit(ScriptBody visitable)
    {
        var result = new AddressedInstructions();
        foreach (var item in visitable.StatementList)
        {
            result.AddRange(item.Accept(this));
        }

        result.Add(new Halt());
        
        return result;
    }

    public AddressedInstructions Visit(LexicalDeclaration visitable)
    {
        var result = new AddressedInstructions();
        foreach (var assignment in visitable.Assignments)
        {
            result.AddRange(assignment.Accept(_expressionVisitor));
        }

        return result;
    }

    public AddressedInstructions Visit(BlockStatement visitable)
    {
        var result = new AddressedInstructions();
        foreach (var item in visitable.StatementList)
        {
            result.AddRange(item.Accept(this));
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

        return new() { new Goto(jumpType) };
    }

    public AddressedInstructions Visit(ExpressionStatement visitable) =>
        visitable.Expression.Accept(_expressionVisitor);

    public AddressedInstructions Visit(ReturnStatement visitable)
    {
        switch (visitable.Expression)
        {
            case null:
                return new() { new Return() };
            case PrimaryExpression primary:
                return new() { new Return(primary.ToValue()) };
        }

        var result = visitable.Expression.Accept(_expressionVisitor);
        var last = new Name(result.OfType<Simple>().Last().Left);
        result.Add(new Return(last));

        return result;
    }

    public AddressedInstructions Visit(ObjectLiteral visitable)
    {
        var objectId = visitable.Id;
        var createObject = new CreateObject(objectId);

        var result = new AddressedInstructions { createObject };

        result.AddRange(visitable.Methods
            .SelectMany(method =>
                method.Accept(this)
            )
        );

        result.AddRange(visitable.Properties
            .SelectMany(property =>
                property.Accept(_expressionVisitor)
            )
        );

        return result;
    }

    public AddressedInstructions Visit(FunctionDeclaration visitable)
    {
        if (!visitable.Statements.Any())
            return new();
        
        var objectId = visitable.Object?.Id;
        var functionInfo = new FunctionInfo(visitable.Name, objectId);

        var result = new AddressedInstructions
        {
            new Goto(functionInfo.End),
            {
                new BeginBlock(BlockType.Function, blockId: functionInfo.ToString()),
                functionInfo.Start.Name
            }
        };
        
        result.AddRange(visitable.Statements.Accept(this));
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
            var last = new Name(result.OfType<Simple>().Last().Left);
            result.Add(new IfNotGoto(last, endBlockLabel));
        }
        
        result.AddRange(visitable.Statement.Accept(this));
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
            return new();

        var blockId = $"if_else_{visitable.GetHashCode()}";
        var startBlockLabel = new Label($"Start_{blockId}");
        var endBlockLabel = new Label($"End_{blockId}");
        
        var result = new AddressedInstructions();
        
        if (visitable.Test is PrimaryExpression primary)
            result.Add(new IfNotGoto(primary.ToValue(), startBlockLabel));
        else
        {
            result.AddRange(visitable.Test.Accept(_expressionVisitor));
            var last = new Name(result.OfType<Simple>().Last().Left);
            result.Add(new IfNotGoto(last,
                visitable.HasElseBlock()
                    ? endBlockLabel
                    : startBlockLabel)
            );
        }
        
        result.AddRange(visitable.Then.Accept(this));
        result.Add(new Goto(endBlockLabel));
        result.Add(new BeginBlock(BlockType.Condition, blockId), startBlockLabel.Name);

        if (visitable.HasElseBlock())
            result.AddRange(visitable.Else.Accept(this));

        result.OfType<Goto>().Where(g => g.JumpType is InsideStatementJumpType.Break)
            .ToList().ForEach(g=> g.SetJump(endBlockLabel));

        result.Add(new EndBlock(BlockType.Condition, blockId), endBlockLabel.Name);

        return result;
    }
}