using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.Semantic.Utils;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class TypeTests
    {
        [Fact]
        public void TypeEqualityTest()
        {
            var number = new Type("number");
            var arrayOfNumbers = new ArrayType(number);
            Assert.False(arrayOfNumbers.Equals(number));
            Assert.False(number.Equals(arrayOfNumbers));
        }

        [Fact]
        public void TypePrecedenceTest()
        {
            Assert.True(TypeUtils.JavaScriptTypes.Number.SubTypeOf(TypeUtils.JavaScriptTypes.Object));
        }

        [Fact]
        public void TypeStringRepresentationTest()
        {
            var matrix = new ArrayType(new ArrayType(new Type("number")));
            
            Assert.Equal("number[][]", matrix.ToString());
        }
    }
}