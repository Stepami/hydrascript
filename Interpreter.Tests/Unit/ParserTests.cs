using Interpreter.Lib.RBNF.Analysis.Syntactic;
using Interpreter.Models;
using Interpreter.Services;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class ParserTests
    {
        private readonly TestContainer container;
        private readonly LexerQueryModel query;

        public ParserTests()
        {
            container = new TestContainer();
            query = new LexerQueryModel("tokenTypes.json");
        }

        private Parser GetParser(string text)
        {
            query.Text = text;
            var lexerCreator = container.Get<ILexerCreatorService>();
            var parserCreator = container.Get<IParserCreatorService>();

            var lexer = lexerCreator.CreateLexer(query);
            var parser = parserCreator.CreateParser(lexer);
            return parser;
        }

        [Theory]
        [ClassData(typeof(ParserSuccessTestData))]
        public void ParserDoesNotThrowTest(string text)
        {
            var parser = GetParser(text);
            
            var ex = Record.Exception(() =>
            {
                // ReSharper disable once UnusedVariable
                var ast = parser.TopDownParse();
            });
            Assert.Null(ex);
        }
    }
}