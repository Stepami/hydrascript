using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Tests.Helpers;

public static class ListExtensions
{
    public static AddressedInstructions ToAddressedInstructions(this List<Instruction> instructions)
    {
        var result = new AddressedInstructions();
        result.AddRange(instructions);
        return result;
    }
}