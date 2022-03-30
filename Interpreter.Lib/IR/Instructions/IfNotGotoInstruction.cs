using System;
using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class IfNotGotoInstruction : GotoInstruction
    {
        private readonly IValue _test;
        
        public IfNotGotoInstruction(IValue test, int jump, int number) :
            base(jump, number)
        {
            _test = test;
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = frames.Peek();
            if (!Convert.ToBoolean(_test.Get(frame)))
            {
                return jump;
            }
            return Number + 1;
        }

        protected override string ToStringRepresentation() => $"IfNot {_test} Goto {Jump()}";
    }
}