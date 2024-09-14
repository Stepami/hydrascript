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

    public static IExecutableInstruction ToInstructionMock(this int number)
    {
        var result = Substitute.For<IExecutableInstruction>();
        result.GetHashCode().Returns(number);
        result.ToString().Returns(number.ToString());
        return result;
    }
}