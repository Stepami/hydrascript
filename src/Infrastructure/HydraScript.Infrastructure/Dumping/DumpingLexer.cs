using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Lexer;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingLexer(
    ILexer lexer,
    IFileSystem fileSystem,
    IOptions<InputFile> inputFile) : ILexer
{
    private readonly InputFile _inputFile = inputFile.Value;

    [ExcludeFromCodeCoverage]
    public IStructure Structure => lexer.Structure;

    public List<Token> GetTokens(string text)
    {
        var tokens = lexer.GetTokens(text);
        var fileName = _inputFile.Info.Name.Split(".js")[0];
        fileSystem.File.WriteAllText(
            $"{fileName}.tokens",
            lexer.ToString());
        return tokens;
    }
}