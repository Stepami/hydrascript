using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Models;

namespace Interpreter.Services
{
    public interface ILexerCreatorService
    {
        Lexer CreateLexer(LexerQueryModel lexerQuery);
    }
}