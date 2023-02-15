using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Data;

namespace Interpreter.Services.Providers.LexerProvider.Impl;

public class LoggingLexer : ILexer
{
    private readonly ILexer _lexer;
    private readonly string _fileName;
    private readonly IFileSystem _fileSystem;

    public LoggingLexer(ILexer lexer, string fileName, IFileSystem fileSystem)
    {
        _lexer = lexer;
        _fileName = fileName;
        _fileSystem = fileSystem;
    }

    [ExcludeFromCodeCoverage]
    public Structure Structure => _lexer.Structure;
    
    public List<Token> GetTokens(string text)
    {
        var tokens = _lexer.GetTokens(text);
        _fileSystem.File.WriteAllText(
            $"{_fileName}.tokens",
            _lexer.ToString()
        );
        return tokens;
    }
}