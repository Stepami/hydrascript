using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizables;

namespace Interpreter.Lib.IR.Optimizers
{
    public class SimpleOptimizer
    {
        private readonly IOptimizable<Instruction> optimizable;

        public SimpleOptimizer()
        {
        }
    }
}