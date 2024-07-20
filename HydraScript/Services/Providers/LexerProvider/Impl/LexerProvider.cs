using System.IO.Abstractions;
using HydraScript.Services.Providers.StructureProvider;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Impl;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Providers.LexerProvider.Impl;

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