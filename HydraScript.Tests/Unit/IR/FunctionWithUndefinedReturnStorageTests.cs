using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;
using Moq;
using Xunit;

namespace HydraScript.Tests.Unit.IR;

public class FunctionWithUndefinedReturnStorageTests
{
    [Fact]
    public void StorageIsEmptyAfterFlushTest()
    {
        const string functionName = nameof(functionName);
        IFunctionWithUndefinedReturnStorage storage = new FunctionWithUndefinedReturnStorage();

        var symbol = new FunctionSymbol(
            id: functionName,
            parameters: new List<Symbol>(),
            "undefined",
            isEmpty: false);

        var decl = new FunctionDeclaration(
            name: new IdentifierReference(functionName),
            returnTypeValue: Mock.Of<TypeValue>(),
            arguments: new List<PropertyTypeValue>(),
            new BlockStatement(new List<StatementListItem>()));

        storage.Save(symbol, decl);

        var declarations = storage.Flush();
        Assert.Contains(decl, declarations);
        
        Assert.Empty(storage.Flush());
    }
}