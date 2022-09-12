using Interpreter.Lib.FrontEnd.TopDownParse.Impl;
using Interpreter.Models;
using Interpreter.Services.Providers;
using Interpreter.Tests.TestData;
using Xunit;

namespace Interpreter.Tests.Unit
{
    public class ParserTests
    {
        private readonly TestContainer _container;
        private readonly StructureModel _structureModel;

        public ParserTests()
        {
            _container = new TestContainer();
            _structureModel = new StructureModel();
        }

        private Parser GetParser()
        {
            var lexerCreator = _container.Get<ILexerProvider>();
            var parserCreator = _container.Get<IParserProvider>();

            var lexer = lexerCreator.CreateLexer(_structureModel);
            var parser = parserCreator.CreateParser(lexer);
            return parser;
        }

        [Theory]
        [ClassData(typeof(ParserSuccessTestData))]
        public void ParserDoesNotThrowTest(string text)
        {
            var parser = GetParser();
            
            var ex = Record.Exception(() =>
            {
                // ReSharper disable once UnusedVariable
                var ast = parser.TopDownParse(text);
            });
            Assert.Null(ex);
        }
    }
}