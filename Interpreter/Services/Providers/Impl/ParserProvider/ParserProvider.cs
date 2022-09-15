using Interpreter.Lib.FrontEnd.TopDownParse;
using Parser = Interpreter.Lib.FrontEnd.TopDownParse.Impl.Parser;
using Microsoft.Extensions.Options;

namespace Interpreter.Services.Providers.Impl.ParserProvider
{
    public class ParserProvider : IParserProvider
    {
        private readonly ILexerProvider _lexerProvider;
        private readonly CommandLineSettings _settings;

        public ParserProvider(ILexerProvider lexerProvider, IOptions<CommandLineSettings> options)
        {
            _lexerProvider = lexerProvider;
            _settings = options.Value;
        }

        public IParser CreateParser()
        {
            var lexer = _lexerProvider.CreateLexer();
            var parser = new Parser(lexer);
            return _settings.Dump
                ? new LoggingParser(parser, _settings.GetInputFileName())
                : parser;
        }
    }
}