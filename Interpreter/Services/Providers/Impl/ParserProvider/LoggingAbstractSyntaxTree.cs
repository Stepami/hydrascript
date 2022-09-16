using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast;

namespace Interpreter.Services.Providers.Impl.ParserProvider
{
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
    
        public List<Instruction> GetInstructions()
        {
            var instructions = _ast.GetInstructions();
            _fileSystem.File.WriteAllLines(
                $"{_fileName}.tac",
                instructions.OrderBy(i => i).Select(i => i.ToString())
            );
            return instructions;
        }
    }
}