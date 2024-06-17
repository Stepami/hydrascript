using System.IO.Abstractions;
using HydraScript.Lib.FrontEnd.TopDownParse;
using HydraScript.Lib.IR.Ast;

namespace HydraScript.Services.Providers.ParserProvider.Impl;

public class LoggingParser : IParser
{
    private readonly IParser _parser;
    private readonly string _fileName;
    private readonly IFileSystem _fileSystem;

    public LoggingParser(IParser parser, string fileName, IFileSystem fileSystem)
    {
        _parser = parser;
        _fileName = fileName;
        _fileSystem = fileSystem;
    }
    
    public IAbstractSyntaxTree TopDownParse(string text)
    {
        var ast = _parser.TopDownParse(text);
        var astDot = ast.ToString();
        _fileSystem.File.WriteAllText("ast.dot", astDot);
        return new LoggingAbstractSyntaxTree(ast, _fileName, _fileSystem);
    }
}