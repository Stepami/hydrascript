using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Models;
using Interpreter.Tests.Stubs;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit.FrontEnd
{
    public class LexerTests
    {
        private readonly Lexer _lexer;

        public LexerTests()
        {
            var mapper = new MapperStub();
            _lexer = new Lexer(mapper.Map<StructureModel, Structure>(new()));
        }

        [Theory]
        [ClassData(typeof(LexerSuccessData))]
        public void LexerDoesNotThrowTest(string text)
        {
            var ex = Record.Exception(() => _lexer.GetTokens(text));
            Assert.Null(ex);
        }
    }
}