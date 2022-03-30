using System.Collections.Generic;
using System.Text;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.VM;
using Type = Interpreter.Lib.Semantic.Types.Type;

namespace Interpreter.Lib.Semantic.Symbols
{
    public class FunctionSymbol : Symbol
    {
        public Type ReturnType { get; init; }

        public List<Symbol> Parameters { get; }

        public FunctionDeclaration Body { get; set; }

        public FunctionInfo CallInfo { get; }

        public FunctionSymbol(string id, IEnumerable<Symbol> parameters) : base(id)
        {
            Parameters = new List<Symbol>(parameters);
            CallInfo = new FunctionInfo(id);
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"function {Id}(");
            sb.AppendJoin(',', Parameters);
            sb.Append($") => {ReturnType}");
            return sb.ToString();
        }
    }
}