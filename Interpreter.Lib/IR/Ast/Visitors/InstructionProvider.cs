using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast.Nodes;
using Interpreter.Lib.IR.Ast.Nodes.Declarations;
using Visitor.NET.Lib.Core;

namespace Interpreter.Lib.IR.Ast.Visitors;

public class InstructionProvider :
    IVisitor<ScriptBody, List<Instruction>>,
    IVisitor<LexicalDeclaration, List<Instruction>>
{
    public List<Instruction> Visit(ScriptBody visitable)
    {
        var result = new List<Instruction>();
        foreach (var listItem in visitable.StatementList)
        {
            result.AddRange(listItem.Accept(this));
        }

        return result;
    }

    public List<Instruction> Visit(LexicalDeclaration visitable)
    {
        var result = new List<Instruction>();
        foreach (var assignment in visitable.Assignments)
        {
            result.AddRange(assignment.Accept(this));
        }

        return result;
    }
}