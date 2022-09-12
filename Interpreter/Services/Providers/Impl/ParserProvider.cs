using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;

namespace Interpreter.Services.Providers.Impl
{
    public class ParserProvider : IParserProvider
    {
        public Parser CreateParser(Lexer lexer) => new (lexer);
    }
}