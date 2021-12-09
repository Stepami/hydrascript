using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class CallInstruction : ThreeAddressCodeInstruction
    {
        private readonly FunctionInfo function;
        private readonly int numberOfArguments;
        
        public CallInstruction(FunctionInfo function, int number, int numberOfArguments, string left = null) :
            base(left, (null, null), "Call ", number)
        {
            this.function = function;
            this.numberOfArguments = numberOfArguments;
        }

        public override int Jump() => function.Location;

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = new Frame(Number + 1, frames.Peek());

            var i = 0;
            var args = new List<(string Id, object Value)>();
            while (i < numberOfArguments)
            {
                args.Add(arguments.Pop());
                frame[args[i].Id] = args[i].Value;
                i++;
            }

            callStack.Push(new Call(Number, function, args, Left));
            frames.Push(frame);
            return function.Location;
        }

        protected override string ToStringRepresentation() => Left == null
            ? $"Call {function}, {numberOfArguments}"
            : $"{Left} = Call {function}, {numberOfArguments}";
    }
}