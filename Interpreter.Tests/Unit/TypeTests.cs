using System.Collections.Generic;
using Interpreter.Lib.IR.CheckSemantics.Types;
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
            var array = new ArrayType(new Type("self"));
            var method = new FunctionType(number, new List<Type> { new("self") });
            var linkedListType = new ObjectType(
                new List<PropertyType>
                {
                    new("data", number),
                    new("wrapped", new ObjectType(new List<PropertyType>
                    {
                        new("next", new Type("self"))
                    })),
                    new("children", array),
                    new("compare", method)
                }
            );
            
            linkedListType.ResolveSelfReferences("self");
            
            Assert.Equal(linkedListType, ((ObjectType)linkedListType["wrapped"])["next"]);
            Assert.Equal(linkedListType, array.Type);
            Assert.Equal(linkedListType, method.Arguments[0]);
        }

        [Fact]
        public void NonSpecifiedTypesVisitingTest()
        {
            var objectType = new ObjectType(
                new List<PropertyType>
                {
                    new("any", new Any()),
                    new("some", new NullType()),
                    new("next", new Type("self")),
                    new("prop", new Type("number"))
                }
            );
            var ex = Record.Exception(() => objectType.ResolveSelfReferences("self"));
            Assert.Null(ex);
            Assert.Equal(objectType["next"], objectType);
        }

        [Fact]
        public void ObjectTypeToStringTest()
        {
            var number = new Type("number");
            var array = new ArrayType(new Type("self"));
            var method = new FunctionType(number, new List<Type> { new("self") });
            var linkedListType = new ObjectType(
                new List<PropertyType>
                {
                    new("data", number),
                    new("wrapped", new ObjectType(new List<PropertyType>
                    {
                        new("next", new Type("self"))
                    })),
                    new("children", array),
                    new("compare", method)
                }
            );
            
            linkedListType.ResolveSelfReferences("self");
            Assert.Contains("@this", linkedListType.ToString());
        }
    }
}