using HydraScript.Lib.FrontEnd.GetTokens.Data;

namespace HydraScript.Lib.FrontEnd.GetTokens;

public interface ILexer
{
    Structure Structure { get; }

    List<Token> GetTokens(string text);
}