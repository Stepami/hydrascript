using System.Collections.Generic;
using System.Reflection;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes;
using Interpreter.Lib.RBNF.Analysis.Syntactic;
using Interpreter.Lib.RBNF.Utils;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class ParserTests
    {
        [Fact]
        public void NextIsTest()
        {
            var lexer = new Mock<IEnumerable<Token>>();
            var domain = GetDomain();
            lexer.Setup(x => x.GetEnumerator()).Returns(TokensMock(domain));

            var parser = new Parser(lexer.Object, domain);

            var nextIsMethod = typeof(Parser).GetMethod("NextIs", BindingFlags.NonPublic | BindingFlags.Instance);
            var currentIsMethod = typeof(Parser).GetMethod("CurrentIs", BindingFlags.NonPublic | BindingFlags.Instance);

            // ReSharper disable once PossibleNullReferenceException
            var nextIsEop = (bool) nextIsMethod.Invoke(parser, new object[] {"EOP"});
            // ReSharper disable once PossibleNullReferenceException
            var currentIsToken = (bool) currentIsMethod.Invoke(parser, new object[] {"token"});
            
            Assert.True(nextIsEop && currentIsToken);
        }

        private static Domain GetDomain()
        {
            var tt1 = new TokenType("token", "token", 1);
            return new Domain(new List<TokenType> {tt1});
        }

        private static IEnumerator<Token> TokensMock(Domain domain)
        {
            return new List<Token>
            {
                new (domain.FindByTag("token")),
                new (LexerUtils.End)
            }.GetEnumerator();
        }
    }
}