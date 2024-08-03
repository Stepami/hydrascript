using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using HydraScript.Infrastructure;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.FrontEnd;

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
        var provider = new Mock<ITokenTypesProvider>();
        provider.Setup(x => x.GetTokenTypes())
            .Returns(tokenTypes);
        var structure = new Structure<GeneratedRegexContainer>(provider.Object);

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