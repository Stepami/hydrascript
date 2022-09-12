using System.Collections.Generic;
using System.IO;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;

namespace Interpreter.Services.Providers.Impl.LexerProvider
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