using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Lexer;
using HydraScript.Domain.FrontEnd.Lexer.Impl;
using HydraScript.Services.Providers.StructureProvider;
using HydraScript.Lib.FrontEnd.GetTokens;
using Microsoft.Extensions.Options;

namespace HydraScript.Services.Providers.LexerProvider.Impl;

public class LexerProvider : ILexerProvider
{
    private readonly IStructureProvider _structureProvider;
    private readonly ITextCoordinateSystemComputer _computer;
    private readonly IFileSystem _fileSystem;
    private readonly CommandLineSettings _settings;

    public LexerProvider(
        IStructureProvider structureProvider,
        ITextCoordinateSystemComputer computer,
        IFileSystem fileSystem,
        IOptions<CommandLineSettings> options)
    {
        _structureProvider = structureProvider;
        _computer = computer;
        _fileSystem = fileSystem;
        _settings = options.Value;
    }

    public ILexer CreateLexer()
    {
        var structure = _structureProvider.CreateStructure();
        var lexer = new RegExpLexer(structure, _computer);
        var inputFileName = _settings.InputFilePath.Split(".js")[0];
        return _settings.Dump
            ? new LoggingLexer(lexer, inputFileName, _fileSystem)
            : lexer;
    }
}