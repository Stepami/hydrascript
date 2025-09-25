using System.Diagnostics.CodeAnalysis;
using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Lexer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingLexer(
    [FromKeyedServices(DecoratorKey.Value)]
    ILexer lexer,
    IFileSystem fileSystem,
    IOptions<FileInfo> inputFile) : ILexer
{
    [ExcludeFromCodeCoverage]
    public IStructure Structure => lexer.Structure;

    public List<Token> GetTokens(string text)
    {
        var tokens = lexer.GetTokens(text);
        var fileName = inputFile.Value.Name.Split(".js")[0];
        fileSystem.File.WriteAllText(
            $"{fileName}.tokens",
            lexer.ToString());
        return tokens;
    }
}