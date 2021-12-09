using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Syntactic;

namespace Interpreter.Services
{
    public class ParserCreatorService : IParserCreatorService
    {
        public Parser CreateParser(Lexer lexer) => new (lexer, lexer.Domain);
    }
}