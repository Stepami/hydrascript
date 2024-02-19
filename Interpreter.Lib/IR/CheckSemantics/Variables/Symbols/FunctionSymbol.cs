using System.Text;
using Interpreter.Lib.IR.CheckSemantics.Types;

namespace Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;

public class FunctionSymbol : Symbol
{
    public override string Id { get; }
    public override FunctionType Type { get; }
    public List<Symbol> Parameters { get; }
    public bool IsEmpty { get; }

    public FunctionSymbol(
        string id,
        IEnumerable<Symbol> parameters,
        FunctionType type,
        bool isEmpty)
    {
        Id = id;
        Parameters = new List<Symbol>(parameters);
        Type = type;
        IsEmpty = isEmpty;
    }

    public override string ToString()
    {
        var sb = new StringBuilder($"function {Id}(");
        sb.AppendJoin(',', Parameters);
        sb.Append($") => {Type.ReturnType}");
        return sb.ToString();
    }
}