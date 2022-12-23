using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements;

public class BreakStatement : InsideLoopStatement
{
    protected override string NodeRepresentation() => "break";

    public override List<Instruction> ToInstructions(int start) =>
        new()
        {
            new Goto(-1, start)
        };
}