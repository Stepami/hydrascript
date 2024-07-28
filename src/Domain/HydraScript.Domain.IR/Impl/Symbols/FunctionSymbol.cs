using System.Text;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class FunctionSymbol(
    string id,
    IEnumerable<ISymbol> parameters,
    Type type,
    bool isEmpty) : Symbol(id, type)
{
    private Type _returnType = type;
    /// <summary>Тип возврата функции</summary>
    public override Type Type => _returnType;

    public IReadOnlyList<ISymbol> Parameters { get; } = new List<ISymbol>(parameters);
    public bool IsEmpty { get; } = isEmpty;

    public void DefineReturnType(Type returnType) =>
        _returnType = returnType;

    public override string ToString() =>
        new StringBuilder($"function {Id}(")
            .AppendJoin(',', Parameters)
            .Append($") => {Type}")
            .ToString();
}