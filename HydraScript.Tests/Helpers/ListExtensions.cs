using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Instructions;

namespace HydraScript.Tests.Helpers;

public static class ListExtensions
{
    public static AddressedInstructions ToAddressedInstructions(this List<Instruction> instructions)
    {
        var result = new AddressedInstructions();
        instructions.ForEach(x => result.Add(x));
        return result;
    }
}