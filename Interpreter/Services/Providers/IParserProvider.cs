using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.TopDownParse.Impl;

namespace Interpreter.Services.Providers
{
    public interface IParserProvider
    {
        Parser CreateParser(Lexer lexer);
    }
}