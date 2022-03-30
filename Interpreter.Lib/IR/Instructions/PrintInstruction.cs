using System;
using System.Collections.Generic;
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

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            Console.Write(_value.Get(frames.Peek()));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"Print {_value}";
    }
}