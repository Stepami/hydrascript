using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl;
using HydraScript.Services.Providers.LexerProvider;
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
        var parser = new TopDownParser(lexer);
        return _settings.Dump
            ? new LoggingParser(parser, _fileSystem)
            : parser;
    }
}