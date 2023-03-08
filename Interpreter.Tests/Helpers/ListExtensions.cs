using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Instructions;

namespace Interpreter.Tests.Helpers;

public static class ListExtensions
{
    public static AddressedInstructions ToAddressedInstructions(this List<Instruction> instructions)
    {
        var result = new AddressedInstructions();
        instructions.ForEach(x => result.Add(x));
        return result;
    }
}