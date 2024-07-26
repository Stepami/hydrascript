using HydraScript.Lib.BackEnd.Impl.Addresses;
using HydraScript.Lib.BackEnd.Impl.Instructions;
using HydraScript.Lib.BackEnd.Impl.Instructions.WithJump;
using HydraScript.Tests.TestData;
using Xunit;

namespace HydraScript.Tests.Unit.BackEnd;

public class InstructionsTests
{
    [Theory]
    [ClassData(typeof(InstructionsData))]
    public void ToStringCorrectTest(Instruction instruction, string expected) =>
        Assert.Equal(expected, instruction.ToString());

    [Fact]
    public void GotoJumpChangedTest()
    {
        var @goto = new Goto(new Label("1"));
        @goto.SetJump(new Label("5"));
        Assert.Equal(new Label("5"), @goto.Execute(vm: new()));
    }
}