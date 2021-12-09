using System.Collections.Generic;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class AsStringInstruction : ThreeAddressCodeInstruction
    {
        public AsStringInstruction(string left, IValue right, int number) :
            base(left, (null, right), "", number)
        {
        }

        public override int Execute(Stack<Call> callStack, Stack<Frame> frames, Stack<(string Id, object Value)> arguments)
        {
            var frame = frames.Peek();
            frame[Left] = right.right.Get(frame).ToString();

            return Jump();
        }

        protected override string ToStringRepresentation() => $"{Left} = {right.right} as string";
    }
}