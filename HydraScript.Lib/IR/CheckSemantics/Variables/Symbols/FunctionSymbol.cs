using System.Text;

namespace HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

public class FunctionSymbol : Symbol
{
    private Type _returnType;
    
    public override string Id { get; }
    /// <summary>Тип возврата функции</summary>
    public override Type Type => _returnType;
    public IReadOnlyList<Symbol> Parameters { get; }
    public bool IsEmpty { get; }

    public FunctionSymbol(
        string id,
        IEnumerable<Symbol> parameters,
        Type returnType,
        bool isEmpty)
    {
        Id = id;
        Parameters = new List<Symbol>(parameters);
        _returnType = returnType;
        IsEmpty = isEmpty;
    }

    public void DefineReturnType(Type returnType) =>
        _returnType = returnType;

    public override string ToString() =>
        new StringBuilder($"function {Id}(")
            .AppendJoin(',', Parameters)
            .Append($") => {Type}")
            .ToString();
}