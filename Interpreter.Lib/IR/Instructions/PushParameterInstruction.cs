using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class PushParameterInstruction : Instruction
    {
        private readonly string parameter;
        private readonly IValue value;

        public PushParameterInstruction(
            int number,
            string parameter,
            IValue value
        ) : base(number)
        {
            this.parameter = parameter;
            this.value = value;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            arguments.Push((parameter, value.Get(frames.Peek())));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"PushParameter {parameter} = {value}";
    }
}