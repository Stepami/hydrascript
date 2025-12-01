using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class ImplicitLiteralTests
{
    [Theory, MemberData(nameof(ToValueDtoData))]
    public void ToValueDto_Always_Expected(ImplicitLiteral implicitLiteral, object? expected)
    {
        var actual = implicitLiteral.ToValueDto().Value;
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void ToValueDto_Undefined_DefaultValueIsNewObject()
    {
        var implicitLiteral = new ImplicitLiteral(TypeIdentValue.Undefined);
        var actual = implicitLiteral.ToValueDto().Value;
        actual.Should().BeOfType<object>();
    }

    public static TheoryData<ImplicitLiteral, object?> ToValueDtoData =>
        new()
        {
            { new ImplicitLiteral(TypeIdentValue.String), string.Empty },
            { new ImplicitLiteral(TypeIdentValue.Number), 0 },
            { new ImplicitLiteral(TypeIdentValue.Boolean), false },
            { new ImplicitLiteral(TypeIdentValue.Null), null },
            { new ImplicitLiteral(new NullableTypeValue(TypeIdentValue.Number)), null },
            { new ImplicitLiteral(new ArrayTypeValue(TypeIdentValue.Number)), new List<object>() }
        };
}