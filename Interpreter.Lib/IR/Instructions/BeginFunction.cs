using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class BeginFunction : Instruction
    {
        private readonly string _name;
        
        public BeginFunction(int number, string name) : base(number)
        {
            _name = name;
        }

        public override int Execute(VirtualMachine vm) => Number + 1;

        protected override string ToStringRepresentation() => $"BeginFunction {_name}";
    }
}