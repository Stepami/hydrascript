using System.Collections.Frozen;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Domain.FrontEnd.Lexer.TokenTypes;
using HydraScript.Infrastructure;

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
                "EOP",
                "ERROR"
            ]);
        Assert.Equal(expectedText, structure.ToString());
    }

    [Theory, AutoHydraScriptData]
    public void GetTokenTypes_NoMatterWhat_AlwaysHaveEopAndError(Structure<DummyContainer> structure)
    {
        var tokenTypes = structure.ToList();
        List<TokenType> expected = [new EndOfProgramType(), new ErrorType()];
        tokenTypes.Should().Contain(expected);
    }
}