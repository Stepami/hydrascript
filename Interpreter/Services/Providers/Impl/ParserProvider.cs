using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Lib.FrontEnd.Parse;

namespace Interpreter.Services.Providers.Impl
{
    public class ParserProvider : IParserProvider
    {
        public Parser CreateParser(Lexer lexer) => new (lexer);
    }
}