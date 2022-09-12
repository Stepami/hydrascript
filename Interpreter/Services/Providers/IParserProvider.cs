using Interpreter.Lib.FrontEnd.Lex;
using Interpreter.Lib.FrontEnd.Parse;

namespace Interpreter.Services.Providers
{
    public interface IParserProvider
    {
        Parser CreateParser(Lexer lexer);
    }
}