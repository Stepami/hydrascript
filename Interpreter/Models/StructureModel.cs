using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Interpreter.Models
{
    [ExcludeFromCodeCoverage]
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