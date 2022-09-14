using Interpreter.Lib.BackEnd.VM;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class Goto : Instruction
    {
        protected int jump;
        
        public Goto(int jump, int number) : base(number)
        {
            this.jump = jump;
        }

        public override int Jump() => jump;

        public override int Execute(VirtualMachine vm) => Jump();

        public void SetJump(int newJump) => jump = newJump;

        protected override string ToStringRepresentation() => $"Goto {Jump()}";
    }
}