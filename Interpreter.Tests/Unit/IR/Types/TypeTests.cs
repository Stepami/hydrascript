using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;
using Xunit;

namespace Interpreter.Tests.Unit.IR.Types;

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
        var calculator = new DefaultValueForTypeCalculator();
        Assert.Null(calculator.GetDefaultValueForType(new NullableType(new Any())));
        Assert.Null(calculator.GetDefaultValueForType(new NullType()));
        Assert.Null(calculator.GetDefaultValueForType(new ObjectType(new List<PropertyType>())));
    }
}