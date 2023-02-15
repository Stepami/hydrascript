using System.IO.Abstractions;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Services.Providers.StructureProvider;
using Microsoft.Extensions.Options;

namespace Interpreter.Services.Providers.LexerProvider.Impl;

public class LexerProvider : ILexerProvider
{
    private readonly IStructureProvider _structureProvider;
    private readonly CommandLineSettings _settings;

    public LexerProvider(IStructureProvider structureProvider, IOptions<CommandLineSettings> options)
    {
        _structureProvider = structureProvider;
        _settings = options.Value;
    }

    public ILexer CreateLexer()
    {
        var structure = _structureProvider.CreateStructure();
        var lexer = new Lexer(structure);
        return _settings.Dump
            ? new LoggingLexer(lexer, _settings.GetInputFileName(), new FileSystem())
            : lexer;
    }
}