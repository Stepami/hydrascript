using System.Collections;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class ReturnInstruction : Instruction, IEnumerable<int>
    {
        private readonly IValue value;
        private readonly List<int> callers = new();

        public int FunctionStart { get; }
        
        public ReturnInstruction(int functionStart, int number, IValue value = null) : base(number)
        {
            this.value = value;
            FunctionStart = functionStart;
        }

        public void AddCaller(int caller) => callers.Add(caller);

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = frames.Pop();
            var call = callStack.Pop();
            if (call.Where != null && value != null)
            {
                frames.Peek()[call.Where] = value.Get(frame);
            }

            return frame.ReturnAddress;
        }

        public IEnumerator<int> GetEnumerator() => callers.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override string ToStringRepresentation() => $"Return{(value != null ? $" {value}" : "")}";
    }
}