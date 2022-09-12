using System.Collections.Generic;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Lib.FrontEnd.GetTokens
{
    public interface ILexer
    {
        Structure Structure { get; }

        List<Token> GetTokens(string text);
    }
}