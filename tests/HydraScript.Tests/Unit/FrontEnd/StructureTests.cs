using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
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
            new ("MyToken", "[m|M][y|Y]", 2),
            new ("OneToSeven", "[1-7]", 1)
        };
        var provider = new Mock<ITokenTypesProvider>();
        provider.Setup(x => x.GetTokenTypes())
            .Returns(tokenTypes);
        var structure = new Structure(provider.Object);

        var expectedText = string.Join('\n',
            new List<string>
            {
                "OneToSeven [1-7]",
                "MyToken [m|M][y|Y]",
                "EOP ",
                "ERROR \\S+"
            }
        );
        Assert.Equal(expectedText,structure.ToString());
    }
}