using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class PopParametersInstruction : Instruction
    {
        private readonly int numberOfArguments;

        public PopParametersInstruction(int number, int numberOfArguments) : base(number)
        {
            this.numberOfArguments = numberOfArguments;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var i = numberOfArguments;
            while (i > 0)
            {
                arguments.Pop();
                i--;
            }

            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"PopParameters {numberOfArguments}";
    }
}