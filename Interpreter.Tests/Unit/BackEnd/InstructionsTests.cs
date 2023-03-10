using Interpreter.Lib.BackEnd.Addresses;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.BackEnd.Instructions.WithJump;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd;

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
        Assert.Equal(new Label("5"), @goto.Execute(vm: null));
    }
}