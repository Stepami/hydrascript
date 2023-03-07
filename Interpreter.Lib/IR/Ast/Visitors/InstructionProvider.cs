using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Nodes.Statements;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class InstructionProvider :
    IVisitor<ScriptBody, AddressedInstructions>,
    IVisitor<LexicalDeclaration, AddressedInstructions>,
    IVisitor<BlockStatement, AddressedInstructions>,
    IVisitor<InsideLoopStatement, AddressedInstructions>,
    IVisitor<ExpressionStatement, AddressedInstructions>,
    IVisitor<ReturnStatement, AddressedInstructions>,
    IVisitor<ObjectLiteral, AddressedInstructions>,
    IVisitor<FunctionDeclaration, AddressedInstructions>,
    IVisitor<WhileStatement, AddressedInstructions>
{
    private readonly ExpressionInstructionProvider _expressionVisitor = new();
    
    public AddressedInstructions Visit(ScriptBody visitable)
    {
        var result = new AddressedInstructions();
        foreach (var item in visitable.StatementList)
        {
            result.AddRange(item.Accept(this));
        }

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

    public AddressedInstructions Visit(InsideLoopStatement visitable)
    {
        var jumpType = visitable.Keyword switch
        {
            InsideLoopStatement.Break => InsideLoopStatementType.Break,
            InsideLoopStatement.Continue => InsideLoopStatementType.Continue,
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
        var objectId = visitable.Parent is AssignmentExpression assignment
            ? assignment.Destination.Id
            : null;
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
        if (!visitable.Any())
            return new();
        
        var objectId = visitable.Object?.Parent is AssignmentExpression assignment
            ? assignment.Destination.Id
            : null;
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

        result.Add(new EndBlock(BlockType.Function, blockId: functionInfo.ToString()));
        
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
            var instructions = visitable.Condition.Accept(_expressionVisitor);
            var last = new Name(instructions.OfType<Simple>().Last().Left);
            result.Add(new IfNotGoto(last, endBlockLabel));
        }
        
        result.AddRange(visitable.Statement.Accept(this));
        result.OfType<Goto>().Where(g => g.JumpType is not null)
            .ToList().ForEach(g =>
            {
                switch (g.JumpType)
                {
                    case InsideLoopStatementType.Break:
                        g.SetJump(endBlockLabel);
                        break;
                    case InsideLoopStatementType.Continue:
                        g.SetJump(startBlockLabel);
                        break;
                }
            });
        result.Add(new Goto(startBlockLabel));

        result.Add(new EndBlock(BlockType.Loop, blockId), endBlockLabel.Name);

        return result;
    }
}