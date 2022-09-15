using System.Collections.Generic;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Data.TokenTypes;
using Xunit;

namespace Interpreter.Tests.Unit.FrontEnd
{
    public class StructureTests
    {
        [Fact]
        public void StructureToStringCorrectTest()
        {
            var tokenTypes = new List<TokenType>
            {
                new ("MyToken", "[m|M][y|Y]", 2),
                new ("OneToSeven", "[1-7]", 1)
            };
            var structure = new Structure(tokenTypes);

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
}