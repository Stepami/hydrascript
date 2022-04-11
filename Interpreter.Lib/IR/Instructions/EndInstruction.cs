using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class EndInstruction : Instruction
    {
        public EndInstruction(int number) : base(number)
        {
        }

        public override bool End() => true;

        public override int Execute(VirtualMachine vm)
        {
            vm.Frames.Pop();
            return -3;
        }

        protected override string ToStringRepresentation() => "End";
    }
}