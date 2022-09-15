using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd
{
    public class InstructionsTests
    {
        [Theory]
        [ClassData(typeof(InstructionsData))]
        public void ToStringCorrectTest(Instruction instruction, string expected) =>
            Assert.Equal(expected, instruction.ToString());
    }
}