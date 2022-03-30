using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class PushParameterInstruction : Instruction
    {
        private readonly string _parameter;
        private readonly IValue _value;

        public PushParameterInstruction(
            int number,
            string parameter,
            IValue value
        ) : base(number)
        {
            _parameter = parameter;
            _value = value;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            arguments.Push((_parameter, _value.Get(frames.Peek())));
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"PushParameter {_parameter} = {_value}";
    }
}