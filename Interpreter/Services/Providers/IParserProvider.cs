using Interpreter.Lib.FrontEnd.TopDownParse;

namespace Interpreter.Services.Providers;

public interface IParserProvider
{
    IParser CreateParser();
}