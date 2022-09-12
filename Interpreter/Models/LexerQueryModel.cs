using System.Collections.Generic;
using AutoMapper;
using Interpreter.Lib.FrontEnd.GetTokens.Impl;
using Interpreter.Lib.FrontEnd.GetTokens.Impl.TokenTypes;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    public class LexerQueryModel
    {
        private List<TokenTypeModel> TokenTypes { get; set; }

        public LexerQueryModel()
        {
            TokenTypes = JsonConvert.DeserializeObject<List<TokenTypeModel>>(
                Interpreter.TokenTypes.Json
            );
        }

        public string Text { get; set; }

        public Structure GetStructure(IMapper mapper) =>
            new(mapper.Map<List<TokenType>>(TokenTypes));
    }
}