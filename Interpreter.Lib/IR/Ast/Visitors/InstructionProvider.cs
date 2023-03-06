using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Nodes.Statements;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class InstructionProvider :
    IVisitor<ScriptBody, AddressedInstructions>,
    IVisitor<LexicalDeclaration, AddressedInstructions>,
    IVisitor<BlockStatement, AddressedInstructions>,
    IVisitor<InsideLoopStatement, AddressedInstructions>
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
}