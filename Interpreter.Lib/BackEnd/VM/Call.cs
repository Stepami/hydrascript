using System.Collections.Generic;
using System.Linq;

namespace Interpreter.Lib.BackEnd.VM
{
    public record Call(
        int From, FunctionInfo To, 
        List<(string Id, object Value)> Parameters,
        string Where = null)
    {
        public override string ToString() =>
            $"{From} => {To.Location}: {To.Id}({string.Join(", ", Parameters.Select(x => $"{x.Id}: {x.Value}"))})";
    }
    
    public record FunctionInfo(string Id, int Location = 0, string MethodOf = null)
    {
        public int Location { get; set; } = Location;

        public string MethodOf { get; set; } = MethodOf;

        public string CallId() =>
            MethodOf == null
                ? Id
                : $"{MethodOf}.{Id}";

        public override string ToString() =>
            $"({Location}, {CallId()})";
    }
}