using Interpreter.Lib.FrontEnd.Parse;
using Interpreter.Models;
using Interpreter.Services.Providers;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class ParserTests
    {
        private readonly TestContainer _container;
        private readonly LexerQueryModel _query;

        public ParserTests()
        {
            _container = new TestContainer();
            _query = new LexerQueryModel();
        }

        private Parser GetParser(string text)
        {
            _query.Text = text;
            var lexerCreator = _container.Get<ILexerProvider>();
            var parserCreator = _container.Get<IParserProvider>();

            var lexer = lexerCreator.CreateLexer(_query);
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