using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR.Optimizables
{
    public class IdentityExpression : IOptimizable<ThreeAddressCodeInstruction>
    {
        public ThreeAddressCodeInstruction Instruction { get; }

        public IdentityExpression(ThreeAddressCodeInstruction instruction)
        {
            Instruction = instruction;
        }

        public bool Test()
        {
            var s = Instruction.ToString();
            return s.Contains("+ 0") || s.Contains("0 +") ||
                   s.Contains("* 1") || s.Contains("1 *") ||
                   s.Contains("- 0") || s.Contains("/ 1");
        }
    }
}