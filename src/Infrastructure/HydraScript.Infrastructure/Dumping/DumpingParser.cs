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
        var astDot = ast.ToString();
        fileSystem.File.WriteAllText("ast.dot", astDot);
        return ast;
    }
}