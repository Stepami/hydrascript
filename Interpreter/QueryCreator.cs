using System.IO;
using Interpreter.Models;

namespace Interpreter
{
    public class QueryCreator
    {
        private readonly Program.Options _options;

        public QueryCreator(Program.Options options)
        {
            _options = options;
        }

        public LexerQueryModel CreateLexerQuery()
        {
            var query = new LexerQueryModel(_options.TokenTypesJsonFilePath);
            var sourceCode = File.ReadAllText(_options.InputFilePath);
            query.Text = sourceCode;
            return query;
        }
    }
}