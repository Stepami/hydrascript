using System.IO;
using Interpreter.Lib.Contracts;
using Interpreter.Lib.FrontEnd.TopDownParse;

namespace Interpreter.Services.Providers.Impl.ParserProvider
{
    public class LoggingParser : IParser
    {
        private readonly IParser _parser;
        private readonly string _fileName;

        public LoggingParser(IParser parser, string fileName)
        {
            _parser = parser;
            _fileName = fileName;
        }
    
        public IAbstractSyntaxTree TopDownParse(string text)
        {
            var ast = _parser.TopDownParse(text);
            var astDot = ast.ToString();
            File.WriteAllText("ast.dot", astDot);
            return new LoggingAbstractSyntaxTree(ast, _fileName);
        }
    }
}