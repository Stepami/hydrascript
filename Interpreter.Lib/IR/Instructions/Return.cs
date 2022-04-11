using System.Collections;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class Return : Instruction, IEnumerable<int>
    {
        private readonly IValue _value;
        private readonly List<int> _callers = new();

        public int FunctionStart { get; }
        
        public Return(int functionStart, int number, IValue value = null) : base(number)
        {
            _value = value;
            FunctionStart = functionStart;
        }

        public void AddCaller(int caller) => _callers.Add(caller);

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Pop();
            var call = vm.CallStack.Pop();
            if (call.Where != null && _value != null)
            {
                vm.Frames.Peek()[call.Where] = _value.Get(frame);
            }

            return frame.ReturnAddress;
        }

        public IEnumerator<int> GetEnumerator() => _callers.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        protected override string ToStringRepresentation() => $"Return{(_value != null ? $" {_value}" : "")}";
    }
}