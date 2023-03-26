using Interpreter.Lib.BackEnd;
using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Microsoft.Extensions.Options;
using Moq;

namespace Interpreter.Tests.Helpers;

public static class MockExtensions
{
    public static Mock<Halt> Trackable(this Mock<Halt> halt)
    {
        halt.Setup(x => x.Execute(It.IsAny<VirtualMachine>()))
            .Returns(new HashAddress(seed: 0)).Verifiable();
        halt.Setup(x => x.End()).Returns(true);
        return halt;
    }

    public static IOptions<CommandLineSettings> ToOptions
        (this Mock<CommandLineSettings> commandLineSettings) =>
        Options.Create(commandLineSettings.Object);

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