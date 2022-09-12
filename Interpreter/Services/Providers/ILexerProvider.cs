using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Models;

namespace Interpreter.Services.Providers
{
    public interface ILexerProvider
    {
        Lexer CreateLexer(StructureModel structureModel);
    }
}