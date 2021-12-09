using System.Collections.Generic;
using AutoMapper;
using Interpreter.Lib.RBNF.Analysis.Lexical;
using Interpreter.Lib.RBNF.Analysis.Lexical.TokenTypes;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    public class LexerQueryModel
    {
        [JsonProperty] private List<TokenTypeModel> TokenTypes { get; set; }

        public string Text { get; set; }

        public Domain GetDomain(IMapper mapper)
        {
            return new(mapper.Map<List<TokenType>>(TokenTypes));
        }
    }
}