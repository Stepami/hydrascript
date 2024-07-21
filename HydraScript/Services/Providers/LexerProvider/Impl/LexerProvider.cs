using System.IO.Abstractions;
using HydraScript.Services.Providers.StructureProvider;
using HydraScript.Lib.FrontEnd.GetTokens;
using HydraScript.Lib.FrontEnd.GetTokens.Impl;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Providers.LexerProvider.Impl;

public class LexerProvider : ILexerProvider
{
    private readonly IStructureProvider _structureProvider;
    private readonly IFileSystem _fileSystem;
    private readonly CommandLineSettings _settings;

    public LexerProvider(
        IStructureProvider structureProvider,
        IFileSystem fileSystem,
        IOptions<CommandLineSettings> options)
    {
        _structureProvider = structureProvider;
        _fileSystem = fileSystem;
        _settings = options.Value;
    }

    public ILexer CreateLexer()
    {
        var structure = _structureProvider.CreateStructure();
        var lexer = new Lexer(structure);
        var inputFileName = _settings.InputFilePath.Split(".js")[0];
        return _settings.Dump
            ? new LoggingLexer(lexer, inputFileName, _fileSystem)
            : lexer;
    }
}