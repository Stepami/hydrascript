using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.TopDownParse;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;

namespace Interpreter.Services.Providers.Impl
{
    public class ParserProvider : IParserProvider
    {
        public IParser CreateParser(ILexer lexer) => new Parser(lexer);
    }
}