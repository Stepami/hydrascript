using System;
using System.Collections.Generic;
using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.IR.Instructions
{
    public class RemoveFromArray : Instruction
    {
        private readonly string _id;
        private readonly IValue _index;
        
        public RemoveFromArray(int number, string id, IValue index) : base(number)
        {
            _id = id;
            _index = index;
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            var list = (List<object>) frame[_id];
            list.RemoveAt(Convert.ToInt32(_index.Get(frame)));
            return Number + 1;
        }

        protected override string ToStringRepresentation() =>
            $"RemoveFrom {_id} at {_index}";
    }
}