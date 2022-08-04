using System.Collections.Generic;
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
        public void TypeStringRepresentationTest()
        {
            var matrix = new ArrayType(new ArrayType(new Type("number")));
            
            Assert.Equal("number[][]", matrix.ToString());
        }

        [Fact]
        public void ObjectTypeEqualityTest()
        {
            var number = new Type("number");
            var p2d1 = new ObjectType(
                new PropertyType[]
                {
                    new("x", number), 
                    new("y", number)
                }
            );
            var p2d2 = new ObjectType(
                new PropertyType[]
                {
                    new("y", number), 
                    new("x", number)
                }
            );
            Assert.Equal(p2d1, p2d2);

            var p3d1 = new ObjectType(
                new PropertyType[]
                {
                    new("a", number),
                    new("x", number),
                    new("y", number)
                }
            );
            var p3d2 = new ObjectType(
                new PropertyType[]
                {
                    new("y", number), 
                    new("x", number),
                    new("z", number)
                }
            );
            Assert.NotEqual(p3d1, p3d2);
            Assert.NotEqual(p3d2, p2d1);
        }

        [Fact]
        public void NullTests()
        {
            var number = new Type("number");
            // ReSharper disable once SuspiciousTypeConversion.Global
            Assert.True(new NullType().Equals(new NullableType(number)));
        }

        [Fact]
        public void TypeWrappingTest()
        {
            var str = new Type("string");
            str = new NullableType(str);
            str = new ArrayType(str);
            Assert.Equal("string?[]", str.ToString());
        }

        [Fact]
        public void DefaultValueTest()
        {
            Assert.Null(TypeUtils.GetDefaultValue(new NullableType(new Any())));
            Assert.Null(TypeUtils.GetDefaultValue(new NullType()));
            Assert.Null(TypeUtils.GetDefaultValue(new ObjectType(new List<PropertyType>())));
        }

        [Fact]
        public void RecursiveTypeTest()
        {
            var number = new Type("number");
            var linkedListType = new ObjectType(
                new List<PropertyType>
                {
                    new("data", number),
                    new("wrapped", new ObjectType(new List<PropertyType>
                    {
                        new("next", new Type("self"))
                    }))
                }
            );
            linkedListType.ResolveSelfReferences("self");
            var wrapper = new ObjectType(new List<PropertyType> {new("data", number)});
            Assert.True(linkedListType.Equals(((ObjectType)linkedListType["wrapped"])["next"]));
            Assert.False(linkedListType.Equals(wrapper));
            Assert.Contains("@this", linkedListType.ToString());
        }
    }
}