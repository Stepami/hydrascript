using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace HydraScript.Infrastructure.Dumping;

internal class DumpingParser(
    [FromKeyedServices(DecoratorKey.Value)] IParser parser, 
    IFileSystem fileSystem) : IParser
{
    public IAbstractSyntaxTree Parse(string text)
    {
        var ast = parser.Parse(text);
        fileSystem.File.WriteAllText("ast.dot", ast.ToString());
        return ast;
    }
}