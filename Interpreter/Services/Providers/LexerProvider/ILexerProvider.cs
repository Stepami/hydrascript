using Interpreter.Lib.FrontEnd.GetTokens;

namespace Interpreter.Services.Providers.LexerProvider;

public interface ILexerProvider
{
    ILexer CreateLexer();
}