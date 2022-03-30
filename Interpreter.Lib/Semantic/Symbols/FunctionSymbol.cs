using System.Collections.Generic;
using System.Text;
using Interpreter.Lib.Semantic.Nodes.Declarations;
using Interpreter.Lib.Semantic.Types;
using Interpreter.Lib.VM;

namespace Interpreter.Lib.Semantic.Symbols
{
    public class FunctionSymbol : Symbol
    {
        public FunctionType Type { get; }
        
        public List<Symbol> Parameters { get; }

        public FunctionDeclaration Body { get; set; }

        public FunctionInfo CallInfo { get; }

        public FunctionSymbol(string id, IEnumerable<Symbol> parameters, FunctionType type) : base(id)
        {
            Parameters = new List<Symbol>(parameters);
            CallInfo = new FunctionInfo(id);
            Type = type;
        }

        public override string ToString()
        {
            var sb = new StringBuilder($"function {Id}(");
            sb.AppendJoin(',', Parameters);
            sb.Append($") => {Type.ReturnType}");
            return sb.ToString();
        }
    }
}