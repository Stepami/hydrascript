using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.IR;
using HydraScript.Domain.IR.Impl;

namespace HydraScript.Application.StaticAnalysis.Impl;

internal class SymbolTableStorage : ISymbolTableStorage
{
    private readonly Dictionary<Guid, ISymbolTable> _symbolTables = [];
    public ISymbolTable this[Scope scope] => _symbolTables[scope.Id];

    public void Init(Scope scope, ISymbolTable symbolTable) =>
        _symbolTables.TryAdd(scope.Id, symbolTable);

    public void InitWithOpenScope(Scope scope)
    {
        ArgumentNullException.ThrowIfNull(scope.OpenScope);
        if (_symbolTables.ContainsKey(scope.Id))
            return;
        var symbolTable = new SymbolTable();
        symbolTable.AddOpenScope(_symbolTables[scope.OpenScope.Id]);
        _symbolTables[scope.Id] = symbolTable;
    }

    public void Clear()
    {
        foreach (var scopeId in _symbolTables.Keys)
        {
            _symbolTables[scopeId].Clear();
        }
        _symbolTables.Clear();
    }
}