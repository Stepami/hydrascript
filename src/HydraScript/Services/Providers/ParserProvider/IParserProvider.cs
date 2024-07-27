using HydraScript.Domain.FrontEnd.Parser;

namespace HydraScript.Services.Providers.ParserProvider;

public interface IParserProvider
{
    IParser CreateParser();
}