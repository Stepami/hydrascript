using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services.Impl;
using Moq;
using Xunit;
using BlockStatement = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements.BlockStatement;
using FunctionDeclaration = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded.FunctionDeclaration;
using IdentifierReference = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions.IdentifierReference;

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
            parameters: [],
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