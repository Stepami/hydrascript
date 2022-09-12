using System.Collections.Generic;
using System.IO;

namespace Interpreter.Lib.FrontEnd.GetTokens.Impl
{
    public class LoggingLexer : ILexer
    {
        private readonly ILexer _lexer;
        private readonly string _fileName;

        public LoggingLexer(ILexer lexer, string fileName)
        {
            _lexer = lexer;
            _fileName = fileName;
        }

        public Structure Structure => _lexer.Structure;
    
        public List<Token> GetTokens(string text)
        {
            var tokens = _lexer.GetTokens(text);
            File.WriteAllText(
                $"{_fileName}.tokens",
                _lexer.ToString()
            );
            return tokens;
        }
    }
}