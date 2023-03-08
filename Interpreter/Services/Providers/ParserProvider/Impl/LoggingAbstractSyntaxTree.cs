using System.IO.Abstractions;
using Interpreter.Lib.BackEnd;
using Interpreter.Lib.IR.Ast;

namespace Interpreter.Services.Providers.ParserProvider.Impl;

public class LoggingAbstractSyntaxTree : IAbstractSyntaxTree
{
    private readonly IAbstractSyntaxTree _ast;
    private readonly string _fileName;
    private readonly IFileSystem _fileSystem;

    public LoggingAbstractSyntaxTree(IAbstractSyntaxTree ast, string fileName, IFileSystem fileSystem)
    {
        _ast = ast;
        _fileName = fileName;
        _fileSystem = fileSystem;
    }
    
    public AddressedInstructions GetInstructions()
    {
        var instructions = _ast.GetInstructions();
        _fileSystem.File.WriteAllLines(
            $"{_fileName}.tac",
            instructions.Select(i => i.ToString())
        );
        return instructions;
    }
}