using HydraScript.Lib.BackEnd;
using HydraScript.Lib.BackEnd.Addresses;
using HydraScript.Lib.BackEnd.Instructions;
using Moq;

namespace HydraScript.Tests.Helpers;

public static class MockExtensions
{
    public static Mock<Halt> Trackable(this Mock<Halt> halt)
    {
        halt.Setup(x => x.Execute(It.IsAny<VirtualMachine>()))
            .Returns(new HashAddress(seed: 0)).Verifiable();
        halt.Setup(x => x.End()).Returns(true);
        return halt;
    }

    public static Mock<Instruction> ToInstructionMock(this int number)
    {
        var result = new Mock<Instruction>(MockBehavior.Default)
        {
            CallBase = true
        };
        
        result.Setup(x => x.GetHashCode())
            .Returns(number);

        result.Setup(x => x.ToString())
            .Returns(number.ToString());

        return result;
    }
}