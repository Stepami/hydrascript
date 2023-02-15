using Interpreter.Lib.FrontEnd.TopDownParse;

namespace Interpreter.Services.Providers.ParserProvider;

public interface IParserProvider
{
    IParser CreateParser();
}