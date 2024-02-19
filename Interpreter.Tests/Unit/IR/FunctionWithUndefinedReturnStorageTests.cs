using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services.Impl;
using Moq;
using Xunit;

namespace Interpreter.Tests.Unit.IR;

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
            new FunctionType(
                "undefined",
                arguments: new List<Type>()),
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