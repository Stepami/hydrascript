using HydraScript.Application.StaticAnalysis;
using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.IR.Impl.Symbols;
using Xunit;
using BlockStatement = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements.BlockStatement;
using FunctionDeclaration = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded.FunctionDeclaration;
using IdentifierReference = HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions.IdentifierReference;

namespace HydraScript.Tests.Unit.IR;

public class FunctionWithUndefinedReturnStorageTests
{
    const string functionName = nameof(functionName);
    [Fact]
    public void StorageIsEmptyAfterFlushTest()
    {
        IFunctionWithUndefinedReturnStorage storage = new FunctionWithUndefinedReturnStorage();

        var symbol = new FunctionSymbol(
            id: functionName,
            parameters: [],
            "undefined",
            isEmpty: false);

        var decl = new FunctionDeclaration(
            name: new IdentifierReference(functionName),
            returnTypeValue: Substitute.For<TypeValue>(),
            arguments: [],
            new BlockStatement([]));

        storage.Save(symbol, decl);

        var declarations = storage.Flush();
        Assert.Contains(decl, declarations);
        
        Assert.Empty(storage.Flush());
    }

    [Fact]
    public void StorageIsCorrectOrderTest()
    {
        FunctionDeclaration[] declarations = [
            new FunctionDeclaration(
                name: new IdentifierReference(functionName),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([])),

            new FunctionDeclaration(
                name: new IdentifierReference(functionName),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([])),

            new FunctionDeclaration(
                name: new IdentifierReference(functionName),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([])),

            new FunctionDeclaration(
                name: new IdentifierReference(functionName),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([]))
        ];
        
        IFunctionWithUndefinedReturnStorage storage = new FunctionWithUndefinedReturnStorage();

        var removable = new FunctionSymbol(
            id: "key2",
            parameters: [],
            "undefined",
            isEmpty: false);

        storage.Save(new FunctionSymbol(
            id: "key1",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[0]);

        storage.Save(removable, declaration: declarations[1]);

        storage.Save(new FunctionSymbol(
            id: "key3",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[2]);

        storage.Save(new FunctionSymbol(
            id: "key4",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[3]);

        storage.RemoveIfPresent(removable);

        Assert.Equal([declarations[0], declarations[2], declarations[3]], storage.Flush());
    }
}