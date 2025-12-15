using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class AsStringTests
{
    [Theory, AutoHydraScriptData]
    public void Execute_String_NoQuotes(TestVirtualMachine vm)
    {
        // Arrange
        AddressedInstructions program = [new AsString(new Constant("string"))];
        vm.ExecuteParams.FrameContext.StepIn();

        // Act
        program[program.Start].Execute(vm.ExecuteParams);

        // Assert
        vm.Frame[program.Start.Name].Should().Be("string");
        vm.Frame[program.Start.Name].Should().NotBe("\"string\"");
    }
}