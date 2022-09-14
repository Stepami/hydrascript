using System.Collections.Generic;
using System.IO;
using System.Linq;
using Interpreter.Lib.BackEnd.Instructions;
using Interpreter.Lib.IR.Ast;

namespace Interpreter.Services.Providers.Impl.ParserProvider
{
    public class LoggingAbstractSyntaxTree : IAbstractSyntaxTree
    {
        private readonly IAbstractSyntaxTree _ast;
        private readonly string _fileName;

        public LoggingAbstractSyntaxTree(IAbstractSyntaxTree ast, string fileName)
        {
            _ast = ast;
            _fileName = fileName;
        }
    
        public List<Instruction> GetInstructions()
        {
            var instructions = _ast.GetInstructions();
            File.WriteAllLines(
                $"{_fileName}.tac",
                instructions.OrderBy(i => i).Select(i => i.ToString())
            );
            return instructions;
        }
    }
}