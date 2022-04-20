using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR.Optimizers
{
    public class IdentityExpression : IOptimizer<Simple>
    {
        public Simple Instruction { get; set; }

        public IdentityExpression(Simple instruction = null)
        {
            Instruction = instruction;
        }

        public bool Test()
        {
            var s = Instruction.ToString();
            return s.Contains("+ 0") || s.Contains("0 +") ||
                   s.Contains("* 1") || s.Contains("1 *") ||
                   s.Contains("- 0") || s.Contains("/ 1") ||
                   s.Contains("-0") || s.Contains("* 0") ||
                   s.Contains("0 *");
        }

        public void Optimize()
        {
            if (Test())
            {
                Instruction.ReduceToAssignment();
            }
        }
    }
}