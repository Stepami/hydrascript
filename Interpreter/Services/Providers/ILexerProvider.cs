using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Models;

namespace Interpreter.Services.Providers
{
    public interface ILexerProvider
    {
        Lexer CreateLexer(LexerQueryModel lexerQuery);
    }
}