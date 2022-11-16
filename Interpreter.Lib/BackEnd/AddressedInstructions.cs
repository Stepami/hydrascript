using System.Collections.Generic;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Lib.BackEnd
{
    public class AddressedInstructions
    {
        private readonly Dictionary<IAddress, Instruction> _instructions = new();
    }
}