using System.Collections.Frozen;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;

namespace HydraScript.UnitTests.Domain.FrontEnd;

public class StructureTests
{
    [Fact]
    public void ToStringCorrectTest()
    {
        var tokenTypes = new List<TokenType>
        {
            new("MyToken"),
            new("OneToSeven")
        }.ToFrozenDictionary(x => x.Tag);
        var provider = Substitute.For<ITokenTypesProvider>();
        provider.GetTokenTypes().Returns(tokenTypes);
        var structure = new Structure<GeneratedRegexContainer>(provider);

        var expectedText = string.Join(
            '\n',
            [
                "MyToken",
                "OneToSeven",
            ]);
        Assert.Equal(expectedText, structure.ToString());
    }

    [Theory, AutoHydraScriptData]
    public void GetTokenTypes_Always_ReturnFromProvider(ITokenTypesProvider provider)
    {
        List<TokenType> expected = [new EndOfProgramType(), new ErrorType()];
        provider.GetTokenTypes().Returns(expected.ToFrozenDictionary(x => x.Tag));
        var structure = new Structure<GeneratedRegexContainer>(provider);
        structure.Should().BeEquivalentTo(expected);
    }
}