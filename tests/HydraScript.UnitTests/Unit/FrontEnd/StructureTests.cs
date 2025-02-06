using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using HydraScript.Infrastructure;
using Xunit;

namespace HydraScript.UnitTests.Unit.FrontEnd;

public class StructureTests
{
    [Fact]
    public void ToStringCorrectTest()
    {
        var tokenTypes = new List<TokenType>
        {
            new("MyToken"),
            new("OneToSeven")
        };
        var provider = Substitute.For<ITokenTypesProvider>();
        provider.GetTokenTypes().Returns(tokenTypes);
        var structure = new Structure<GeneratedRegexContainer>(provider);

        var expectedText = string.Join(
            '\n',
            [
                "MyToken",
                "OneToSeven",
                "EOP",
                "ERROR"
            ]);
        Assert.Equal(expectedText, structure.ToString());
    }
}