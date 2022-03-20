using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    public class LexerQueryModel
    {
        [JsonProperty] private List<TokenTypeModel> TokenTypes { get; set; }

        public LexerQueryModel()
        {
            TokenTypes = new List<TokenTypeModel>();
            Text = "";
        }

        public LexerQueryModel(string tokenTypesJsonFilePath)
        {
            TokenTypes = JsonConvert.DeserializeObject<List<TokenTypeModel>>(
                File.ReadAllText(tokenTypesJsonFilePath)
            );
        }

        public string Text { get; set; }

        public Structure GetDomain(IMapper mapper)
        {
            return new(mapper.Map<List<TokenType>>(TokenTypes));
        }
    }
}