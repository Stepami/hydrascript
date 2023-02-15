using Interpreter.Lib.IR.Ast;
using Interpreter.Services.Providers.ParserProvider;

namespace Interpreter.Services.Parsing.Impl;

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
        return parser.TopDownParse(text);
    }
}