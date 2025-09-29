using System.Diagnostics.CodeAnalysis;
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

    public List<Token> GetTokens(string text)
    {
        var tokens = lexer.GetTokens(text);
        dumpingService.Dump(lexer.ToString(), "tokens");
        return tokens;
    }
}