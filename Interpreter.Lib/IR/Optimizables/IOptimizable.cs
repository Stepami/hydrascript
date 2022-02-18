using System;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.IR.Optimizables
{
    public interface IOptimizable<out T> where T : Instruction
    {
        T Instruction { get; }
        bool Test();
    }
}