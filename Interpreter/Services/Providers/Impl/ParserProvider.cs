using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Syntactic;

namespace Interpreter.Services.Providers.Impl
{
    public class ParserProvider : IParserProvider
    {
        public Parser CreateParser(Lexer lexer) => new (lexer);
    }
}