using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR.Optimizers
{
    public interface IOptimizer<out T> where T : Instruction
    {
        T Instruction { get; }
        
        bool Test();

        void Optimize();

        bool CanOptimize(Instruction instruction) => 
            typeof(T) == instruction.GetType();
    }
}