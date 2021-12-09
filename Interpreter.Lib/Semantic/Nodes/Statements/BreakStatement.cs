using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class BreakStatement : InsideLoopStatement
    {
        protected override string NodeRepresentation() => "break";

        public override List<Instruction> ToInstructions(int start) =>
            new()
            {
                new GotoInstruction(-1, start)
            };
    }
}