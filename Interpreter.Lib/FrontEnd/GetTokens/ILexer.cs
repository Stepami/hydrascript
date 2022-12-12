using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Lib.FrontEnd.GetTokens;

public interface ILexer
{
    Structure Structure { get; }

    List<Token> GetTokens(string text);
}