using System;
using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Values;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class IndexAssignment : Simple
    {
        public IndexAssignment(string left, (IValue left, IValue right) right, int number) : 
            base(left, right, "[]", number)
        {
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            var obj = (List<object>) frame[Left];
            var index = Convert.ToInt32(right.left.Get(frame));
            obj[index] = right.right.Get(frame);
            return Number + 1;
        }

        protected override string ToStringRepresentation() =>
            $"{Left}[{right.left}] = {right.right}";
    }
}