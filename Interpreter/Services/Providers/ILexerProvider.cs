using Interpreter.Lib.FrontEnd.GetTokens;

namespace Interpreter.Services.Providers;

public interface ILexerProvider
{
    ILexer CreateLexer();
}