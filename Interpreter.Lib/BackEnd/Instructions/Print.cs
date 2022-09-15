using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class Print : Instruction
    {
        private readonly IValue _value;
        
        public Print(int number, IValue value) : base(number)
        {
            _value = value;
        }

        public override int Execute(VirtualMachine vm)
        {
            vm.Writer.WriteLine(_value.Get(vm.Frames.Peek()));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"Print {_value}";
    }
}