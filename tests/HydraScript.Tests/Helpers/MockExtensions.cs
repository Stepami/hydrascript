using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Addresses;
using HydraScript.Domain.BackEnd.Impl.Instructions;

namespace HydraScript.Tests.Helpers;

public static class MockExtensions
{
    public static Mock<Halt> Trackable(this Mock<Halt> halt)
    {
        halt.Setup(x => x.Execute(It.IsAny<IExecuteParams>()))
            .Returns(new HashAddress(seed: 0)).Verifiable();
        halt.Setup(x => x.End).Returns(true);
        return halt;
    }

    public static Mock<IExecutableInstruction> ToInstructionMock(this int number)
    {
        var result = new Mock<IExecutableInstruction>();
        result.SetupAllProperties();
        
        result.Setup(x => x.GetHashCode())
            .Returns(number);

        result.Setup(x => x.ToString())
            .Returns(number.ToString());

        return result;
    }
}