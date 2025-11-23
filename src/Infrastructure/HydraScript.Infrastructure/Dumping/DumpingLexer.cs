using System.Diagnostics.CodeAnalysis;
using Cysharp.Text;
using HydraScript.Domain.FrontEnd.Lexer;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingLexer(
    [FromKeyedServices(DecoratorKey.Value)]
    ILexer lexer,
    IDumpingService dumpingService) : ILexer
{
    [ExcludeFromCodeCoverage]
    public IStructure Structure => lexer.Structure;

    public IEnumerable<Token> GetTokens(string text)
    {
        var tokens = lexer.GetTokens(text).ToList();
        dumpingService.Dump(ZString.Join('\n', tokens), "tokens");
        return tokens;
    }
}