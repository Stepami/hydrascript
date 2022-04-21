using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR.Optimizers
{
    public class ZeroExpression : IOptimizer<Simple>
    {
        public Simple Instruction { get; }

        public ZeroExpression(Simple instruction)
        {
            Instruction = instruction;
        }

        public bool Test()
        {
            var s = Instruction.ToString().Split('=')[1].Trim();
            return s == "-0" ||
                   s.EndsWith("* 0") || s.StartsWith("0 *");
        }

        public void Optimize()
        {
            if (Test())
            {
                Instruction.ReduceToZero();
            }
        }
    }
}