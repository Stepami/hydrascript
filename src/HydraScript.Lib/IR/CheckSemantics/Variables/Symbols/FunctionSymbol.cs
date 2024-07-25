using System.Text;

namespace HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;

public class FunctionSymbol(
    string id,
    IEnumerable<Symbol> parameters,
    Type type,
    bool isEmpty) : Symbol
{
    public override string Id { get; } = id;

    /// <summary>Тип возврата функции</summary>
    public override Type Type => type;
    public IReadOnlyList<Symbol> Parameters { get; } = new List<Symbol>(parameters);
    public bool IsEmpty { get; } = isEmpty;

    public void DefineReturnType(Type returnType) =>
        type = returnType;

    public override string ToString() =>
        new StringBuilder($"function {Id}(")
            .AppendJoin(',', Parameters)
            .Append($") => {Type}")
            .ToString();
}