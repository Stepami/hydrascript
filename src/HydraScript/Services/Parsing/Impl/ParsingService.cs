using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Services.Providers.ParserProvider;
using HydraScript.Lib.IR.Ast;

namespace HydraScript.Services.Parsing.Impl;

public class ParsingService : IParsingService
{
    private readonly IParserProvider _parserProvider;

    public ParsingService(IParserProvider parserProvider)
    {
        _parserProvider = parserProvider;
    }
    
    public IAbstractSyntaxTree Parse(string text)
    {
        var parser = _parserProvider.CreateParser();
        return parser.Parse(text);
    }
}