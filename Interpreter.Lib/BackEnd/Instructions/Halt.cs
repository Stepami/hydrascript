namespace Interpreter.Lib.BackEnd.Instructions
{
    public class Halt : Instruction
    {
        public Halt(int number) : base(number)
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