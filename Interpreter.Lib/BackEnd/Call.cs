using System.Collections.Generic;
using System.Linq;

namespace Interpreter.Lib.BackEnd
{
    public record Call(
        int From, FunctionInfo To, 
        List<(string Id, object Value)> Parameters,
        string Where = null)
    {
        public override string ToString() =>
            $"{From} => {To.Location}: {To.Id}({string.Join(", ", Parameters.Select(x => $"{x.Id}: {x.Value}"))})";
    }
}