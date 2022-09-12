using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Models;

namespace Interpreter.Services.Providers
{
    public interface ILexerProvider
    {
        Lexer CreateLexer(LexerQueryModel lexerQuery);
    }
}