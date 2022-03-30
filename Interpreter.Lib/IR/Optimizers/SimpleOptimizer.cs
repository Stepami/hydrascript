using Interpreter.Lib.IR.Instructions;
using Interpreter.Lib.IR.Optimizables;

namespace Interpreter.Lib.IR.Optimizers
{
    public class SimpleOptimizer
    {
        private readonly IOptimizable<Instruction> _optimizable;

        public SimpleOptimizer()
        {
        }
    }
}