using System.IO;
using Newtonsoft.Json.Linq;
using Interpreter.Models;

namespace Interpreter
{
    public class QueryCreator
    {
        private readonly Program.Options options;

        public QueryCreator(Program.Options options)
        {
            this.options = options;
        }

        public LexerQueryModel CreateLexerQuery()
        {
            var query = new LexerQueryModel(options.TokenTypesJsonFilePath);
            var sourceCode = File.ReadAllText(options.InputFilePath);
            query.Text = sourceCode;
            return query;
        }
    }
}