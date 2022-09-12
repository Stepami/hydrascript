using System.Collections.Generic;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    public class StructureModel
    {
        public List<TokenTypeModel> TokenTypes { get; }

        public StructureModel()
        {
            TokenTypes = JsonConvert.DeserializeObject<List<TokenTypeModel>>(
                Interpreter.TokenTypes.Json
            );
        }
    }
}