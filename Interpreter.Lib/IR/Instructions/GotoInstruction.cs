using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class GotoInstruction : Instruction
    {
        protected int jump;
        
        public GotoInstruction(int jump, int number) : base(number)
        {
            this.jump = jump;
        }

        public override int Jump() => jump;

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments) => Jump();

        public void SetJump(int newJump) => jump = newJump;

        protected override string ToStringRepresentation() => $"Goto {Jump()}";
    }
}