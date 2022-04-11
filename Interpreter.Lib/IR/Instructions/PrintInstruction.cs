using System;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class PrintInstruction : Instruction
    {
        private readonly IValue _value;
        
        public PrintInstruction(int number, IValue value) : base(number)
        {
            _value = value;
        }

        public override int Execute(VirtualMachine vm)
        {
            Console.Write(_value.Get(vm.Frames.Peek()));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"Print {_value}";
    }
}