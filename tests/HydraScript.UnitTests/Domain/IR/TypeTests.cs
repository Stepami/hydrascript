using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Domain.IR;

public class TypeTests
{
    [Fact]
    public void Equals_DifferentTypeCombinations_ReturnsExpectedResult()
    {
        var number = new Type("number");
        var arrayOfNumbers = new ArrayType(number);
        var nullableNumber = new NullableType(number);
        Assert.False(arrayOfNumbers.Equals(number));
        Assert.False(number.Equals(arrayOfNumbers));
        Assert.True(nullableNumber.Equals(number));
    }

    [Fact]
    public void ToString_ArrayOfArray_ReturnsExpectedRepresentation()
    {
        var matrix = new ArrayType(new ArrayType(new Type("number")));
            
        Assert.Equal("number[][]", matrix.ToString());
    }

    [Fact]
    public void Equals_NullComparedWithNullable_ReturnsTrue()
    {
        var number = new Type("number");
        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.True(new NullType().Equals(new NullableType(number)));
    }

    [Fact]
    public void ToString_WrappedNullableStringArray_ReturnsExpectedRepresentation()
    {
        var str = new Type("string");
        str = new NullableType(str);
        str = new ArrayType(str);
        Assert.Equal("string?[]", str.ToString());
    }
}
