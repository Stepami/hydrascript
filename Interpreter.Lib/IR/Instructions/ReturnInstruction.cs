using System.Collections;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class ReturnInstruction : Instruction, IEnumerable<int>
    {
        private readonly IValue _value;
        private readonly List<int> _callers = new();

        public int FunctionStart { get; }
        
        public ReturnInstruction(int functionStart, int number, IValue value = null) : base(number)
        {
            _value = value;
            FunctionStart = functionStart;
        }

        public void AddCaller(int caller) => _callers.Add(caller);

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = frames.Pop();
            var call = callStack.Pop();
            if (call.Where != null && _value != null)
            {
                frames.Peek()[call.Where] = _value.Get(frame);
            }

            return frame.ReturnAddress;
        }

        public IEnumerator<int> GetEnumerator() => _callers.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override string ToStringRepresentation() => $"Return{(_value != null ? $" {_value}" : "")}";
    }
}