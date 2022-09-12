using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.TopDownParse;

namespace Interpreter.Services.Providers
{
    public interface IParserProvider
    {
        Parser CreateParser(Lexer lexer);
    }
}