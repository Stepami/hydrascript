using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Models;

namespace Interpreter.Services.Providers
{
    public interface ILexerProvider
    {
        Lexer CreateLexer(LexerQueryModel lexerQuery);
    }
}