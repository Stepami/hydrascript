using HydraScript.Lib.FrontEnd.GetTokens;

namespace HydraScript.Services.Providers.LexerProvider;

public interface ILexerProvider
{
    ILexer CreateLexer();
}