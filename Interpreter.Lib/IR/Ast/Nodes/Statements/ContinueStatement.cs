using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.IR.Ast.Nodes.Statements
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