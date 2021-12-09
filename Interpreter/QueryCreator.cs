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
            var queryObj = new JObject
            {
                ["tokenTypes"] = JToken.Parse(File.ReadAllText(options.TokenTypesJsonFilePath)),
                ["text"] = options.InputFilePath == null ? "" : File.ReadAllText(options.InputFilePath)
            };
            return queryObj.ToObject<LexerQueryModel>();
        }
    }
}