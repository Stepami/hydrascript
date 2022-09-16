using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.IR.Ast
{
    public interface IAbstractSyntaxTree
    {
        List<Instruction> GetInstructions();
    }
}