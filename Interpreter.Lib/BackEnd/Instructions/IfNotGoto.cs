using System;
using Interpreter.Lib.BackEnd.Values;
using Interpreter.Lib.BackEnd.VM;

namespace Interpreter.Lib.BackEnd.Instructions
{
    public class IfNotGoto : Goto
    {
        private readonly IValue _test;
        
        public IfNotGoto(IValue test, int jump, int number) :
            base(jump, number)
        {
            _test = test;
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            if (!Convert.ToBoolean(_test.Get(frame)))
            {
                return jump;
            }
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"IfNot {_test} Goto {Jump()}";
    }
}