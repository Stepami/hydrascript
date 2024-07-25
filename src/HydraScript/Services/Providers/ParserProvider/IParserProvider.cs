using HydraScript.Lib.FrontEnd.TopDownParse;

namespace HydraScript.Services.Providers.ParserProvider;

public interface IParserProvider
{
    IParser CreateParser();
}