using HydraScript.Domain.BackEnd;
using HydraScript.Domain.BackEnd.Impl;
using HydraScript.Domain.BackEnd.Impl.Instructions.WithAssignment;
using HydraScript.Domain.BackEnd.Impl.Values;

namespace HydraScript.UnitTests.Domain.BackEnd;

public class AsStringTests
{
    [Theory, AutoHydraScriptData]
    public void Execute_String_NoQuotes(VirtualMachine vm)
    {
        // Arrange
        AddressedInstructions program = [new AsString(new Constant("string"))];
        vm.ExecuteParams.Frames.Push(new Frame(program.Start));

        // Act
        program[program.Start].Execute(vm.ExecuteParams);

        // Assert
        var frame = vm.ExecuteParams.Frames.Peek();
        frame[program.Start.Name].Should().Be("string");
        frame[program.Start.Name].Should().NotBe("\"string\"");
    }
}