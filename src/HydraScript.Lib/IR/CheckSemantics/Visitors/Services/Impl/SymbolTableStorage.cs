using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.CheckSemantics.Variables;
using HydraScript.Lib.IR.CheckSemantics.Variables.Impl;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;

public class SymbolTableStorage : ISymbolTableStorage
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
}