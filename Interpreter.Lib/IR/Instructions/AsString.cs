using Interpreter.Lib.VM;
using Interpreter.Lib.VM.Values;

namespace Interpreter.Lib.IR.Instructions
{
    public class AsString : Simple
    {
        public AsString(string left, IValue right, int number) :
            base(left, (null, right), "", number)
        {
        }

        public override int Execute(VirtualMachine vm)
        {
            var frame = vm.Frames.Peek();
            frame[Left] = right.right.Get(frame).ToString();

            return Jump();
        }

        protected override string ToStringRepresentation() => $"{Left} = {right.right} as string";
    }
}