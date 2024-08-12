using System.IO.Abstractions;
using HydraScript.Domain.FrontEnd.Parser;

namespace HydraScript.Infrastructure.Logging;

internal class LoggingParser(IParser parser, IFileSystem fileSystem) : IParser
{
    public IAbstractSyntaxTree Parse(string text)
    {
        var ast = parser.Parse(text);
        var astDot = ast.ToString();
        fileSystem.File.WriteAllText("ast.dot", astDot);
        return ast;
    }
}