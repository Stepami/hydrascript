using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.TopDownParse;

namespace Interpreter.Services.Providers.Impl
{
    public class ParserProvider : IParserProvider
    {
        public Parser CreateParser(Lexer lexer) => new (lexer);
    }
}