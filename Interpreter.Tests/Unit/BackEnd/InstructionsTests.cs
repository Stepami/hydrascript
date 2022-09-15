using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Tests.TestData;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd
{
    public class InstructionsTests
    {
        [Theory]
        [ClassData(typeof(InstructionsData))]
        public void ToStringCorrectTest(Instruction instruction, string expected) =>
            Assert.Equal(expected, instruction.ToString());

        [Fact]
        public void ComparisonDependsOnAddressTest()
        {
            var instruction1 = new Mock<Instruction>(1).Object;
            var instruction2 = new Mock<Instruction>(2).Object;

            Assert.Equal(1, instruction2.CompareTo(instruction1));
        }

        [Fact]
        public void GotoJumpChangedTest()
        {
            var @goto = new Goto(0, 1);
            @goto.SetJump(5);
            Assert.Equal(5, @goto.Jump());
        }

        [Fact]
        public void ReturnCallersAddedTest()
        {
            var @return = new Return(7, 19);
            @return.AddCaller(@return.FunctionStart - 2);
            Assert.NotEmpty(@return);
        }
    }
}