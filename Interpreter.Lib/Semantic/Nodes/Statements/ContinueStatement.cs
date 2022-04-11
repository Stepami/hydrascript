using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Semantic.Nodes.Statements
{
    public class ContinueStatement : InsideLoopStatement
    {
        protected override string NodeRepresentation() => "continue";
        
        public override List<Instruction> ToInstructions(int start) =>
            new()
            {
                new Goto(-2, start)
            };
    }
}