using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Syntactic;

namespace Interpreter.Services.Providers
{
    public interface IParserProvider
    {
        Parser CreateParser(Lexer lexer);
    }
}