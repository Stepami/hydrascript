using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Application;

public class HydraScriptTypesServiceTests
{
    private readonly HydraScriptTypesService _typesService = new();

    [Theory, MemberData(nameof(ConversionsData))]
    public void IsExplicitCastAllowed_Always_Success(Type from, Type to, bool expected) =>
        _typesService.IsExplicitCastAllowed(from, to).Should().Be(expected);

    public static TheoryData<Type, Type, bool> ConversionsData =>
        new()
        {
            { new ObjectType([]), "string", true },
            { "number", new NullableType("number"), true },
            { new NullableType("number"), "number", true },
            { "string", "number", true },
            { "string", "boolean", true },
            { "boolean", "number", true },
            { "number", "boolean", true },
            { new ObjectType([]), "boolean", false },
            { new ArrayType("number"), "number", false },
            { new ArrayType("number"), new NullableType("boolean"), false },
        };

    [Fact]
    public void GetDefaultValueForType_NullableTypes_ReturnsNull()
    {
        var calculator = new HydraScriptTypesService();
        Assert.Null(calculator.GetDefaultValueForType(new NullableType(new Any())));
        Assert.Null(calculator.GetDefaultValueForType(new NullType()));
        Assert.Null(calculator.GetDefaultValueForType(new ObjectType([])));
    }
}