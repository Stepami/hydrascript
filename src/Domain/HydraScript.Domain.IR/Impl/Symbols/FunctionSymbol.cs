using System.Diagnostics.CodeAnalysis;
using System.Text;
using HydraScript.Domain.IR.Impl.Symbols.Ids;

namespace HydraScript.Domain.IR.Impl.Symbols;

public class FunctionSymbol(
    string name,
    IReadOnlyCollection<ISymbol> parameters,
    Type type,
    bool isEmpty) : Symbol(name, type)
{
    private Type _returnType = type;
    /// <summary>Тип возврата функции</summary>
    public override Type Type => _returnType;

    /// <summary>
    /// Перегрузка функции
    /// </summary>
    public override FunctionSymbolId Id { get; } = new(name, parameters.Select(x => x.Type));

    public IReadOnlyList<ISymbol> Parameters { get; } = new List<ISymbol>(parameters);
    public bool IsEmpty { get; } = isEmpty;

    public void DefineReturnType(Type returnType) =>
        _returnType = returnType;

    [ExcludeFromCodeCoverage]
    public override string ToString() =>
        new StringBuilder($"function {Name}(")
            .AppendJoin(',', Parameters)
            .Append($") => {Type}")
            .ToString();

    public static bool operator <(FunctionSymbol left, FunctionSymbol right) =>
        left.Parameters.Count < right.Parameters.Count;

    public static bool operator >(FunctionSymbol left, FunctionSymbol right) =>
        left.Parameters.Count > right.Parameters.Count;
}