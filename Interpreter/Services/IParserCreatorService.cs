using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Syntactic;

namespace Interpreter.Services
{
    public interface IParserCreatorService
    {
        Parser CreateParser(Lexer lexer);
    }
}