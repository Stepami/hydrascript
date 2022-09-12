using System.Collections.Generic;
using Interpreter.Lib.IR.Instructions;

namespace Interpreter.Lib.Contracts
{
    public interface IAbstractSyntaxTree
    {
        List<Instruction> GetInstructions();
    }
}