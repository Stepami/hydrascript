using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class ComplexLiteralTests(ITestOutputHelper testOutputHelper)
{
    [Theory,  MemberData(nameof(StartsWithData))]
    public void Id_DifferentType_StartsWithExpected(ComplexLiteral complexLiteral, string expectedPrefix)
    {
        var complexLiteralId = complexLiteral.Id;
        testOutputHelper.WriteLine(complexLiteralId);
        complexLiteralId.Name.Should().StartWith(expectedPrefix);
    }

    public static TheoryData<ComplexLiteral, string> StartsWithData =>
        new()
        {
            { new ArrayLiteral([]), "_t_arr_" },
            { new ObjectLiteral([]), "_t_obj_" }
        };
}