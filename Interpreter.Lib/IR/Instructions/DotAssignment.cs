using System.Collections.Generic;
using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.IR.Instructions
{
    public class DotAssignment : Simple
    {
        public DotAssignment(string left, (IValue left, IValue right) right, int number) : 
            base(left, right, ".", number)
        {
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            var obj = (Dictionary<string, object>) frame[Left];
            var field = right.left.ToString() ?? string.Empty;
            obj[field] = right.right.Get(frame);
            return Number + 1;
        }

        protected override string ToStringRepresentation() =>
            $"{Left}{@operator}{right.left} = {right.right}";
    }
}