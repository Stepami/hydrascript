using System;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class PrintInstruction : Instruction
    {
        private readonly IValue value;
        
        public PrintInstruction(int number, IValue value) : base(number)
        {
            this.value = value;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            Console.Write(value.Get(frames.Peek()));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"Print {value}";
    }
}