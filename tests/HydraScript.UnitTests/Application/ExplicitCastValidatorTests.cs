using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Application;

public class ExplicitCastValidatorTests
{
    private readonly ExplicitCastValidator _explicitCastValidator = new();

    [Theory, MemberData(nameof(ConversionsData))]
    public void IsAllowed_Always_Success(Type from, Type to, bool expected) =>
        _explicitCastValidator.IsAllowed(from, to).Should().Be(expected);

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
}