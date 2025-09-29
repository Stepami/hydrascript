using HydraScript.Domain.FrontEnd.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingParser(
    [FromKeyedServices(DecoratorKey.Value)] IParser parser, 
    IDumpingService dumpingService) : IParser
{
    public IAbstractSyntaxTree Parse(string text)
    {
        var ast = parser.Parse(text);
        dumpingService.Dump(ast.ToString(), "dot");
        return ast;
    }
}