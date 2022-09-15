using Interpreter.Lib.BackEnd.Values;
using Xunit;

namespace Interpreter.Tests.Unit.BackEnd
{
    public class ValuesTests
    {
        [Fact]
        public void ConstantNotEqualToNameTest()
        {
            var name = new Name("a");
            var constant = new Constant("a", "a");
            Assert.False(name.Equals(constant));
            Assert.False(constant.Equals(name));
        }

        [Fact]
        public void ValueToStringCorrect()
        {
            var name = new Name("bbb");
            var constant = new Constant(1, "1.0");
            Assert.Equal("bbb", name.ToString());
            Assert.Equal("1.0", constant.ToString());
        }
    }
}