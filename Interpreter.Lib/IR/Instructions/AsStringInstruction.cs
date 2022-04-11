using Interpreter.Lib.VM;

namespace Interpreter.Lib.IR.Instructions
{
    public class AsStringInstruction : ThreeAddressCodeInstruction
    {
        public AsStringInstruction(string left, IValue right, int number) :
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