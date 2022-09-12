using System.Collections.Generic;
using Interpreter.Lib.FrontEnd.Lex;

namespace Interpreter.Lib.Contracts
{
    public interface ILexer
    {
        Structure Structure { get; }

        List<Token> GetTokens(string text);
    }
}