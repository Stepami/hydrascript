using System.IO.Abstractions;
using HydraScript.Services.Providers.LexerProvider;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.FrontEnd.TopDownParse.Impl;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Providers.ParserProvider.Impl;

public class ParserProvider : IParserProvider
{
    private readonly ILexerProvider _lexerProvider;
    private readonly IFileSystem _fileSystem;
    private readonly CommandLineSettings _settings;

    public ParserProvider(
        ILexerProvider lexerProvider,
        IFileSystem fileSystem,
        IOptions<CommandLineSettings> options)
    {
        _lexerProvider = lexerProvider;
        _fileSystem = fileSystem;
        _settings = options.Value;
    }

    public IParser CreateParser()
    {
        var lexer = _lexerProvider.CreateLexer();
        var parser = new Parser(lexer);
        var inputFileName = _settings.InputFilePath.Split(".js")[0];
        return _settings.Dump
            ? new LoggingParser(parser, inputFileName, _fileSystem)
            : parser;
    }
}