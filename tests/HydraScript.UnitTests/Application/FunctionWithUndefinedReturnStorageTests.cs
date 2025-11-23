using HydraScript.Application.StaticAnalysis;
using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;
using HydraScript.Domain.IR.Impl.Symbols;

namespace HydraScript.UnitTests.Application;

public class FunctionWithUndefinedReturnStorageTests
{
    private const string FunctionName = nameof(FunctionName);

    [Fact]
    public void StorageIsEmptyAfterFlushTest()
    {
        IFunctionWithUndefinedReturnStorage storage = new FunctionWithUndefinedReturnStorage();

        var symbol = new FunctionSymbol(
            name: FunctionName,
            parameters: [],
            "undefined",
            isEmpty: false);

        var decl = new FunctionDeclaration(
            name: new IdentifierReference(FunctionName),
            returnTypeValue: Substitute.For<TypeValue>(),
            arguments: [],
            new BlockStatement([]),
            indexOfFirstDefaultArgument: int.MaxValue);

        storage.Save(symbol, decl);

        var declarations = storage.Flush().ToArray();
        Assert.Contains(decl, declarations);
        Assert.Empty(storage.Flush());
    }

    [Fact]
    public void StorageIsCorrectOrderTest()
    {
        FunctionDeclaration[] declarations = [
            new(
                name: new IdentifierReference(FunctionName + "1"),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([]),
                indexOfFirstDefaultArgument: int.MaxValue),

            new(
                name: new IdentifierReference(FunctionName + "2"),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([]),
                indexOfFirstDefaultArgument: int.MaxValue),

            new(
                name: new IdentifierReference(FunctionName + "3"),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([]),
                indexOfFirstDefaultArgument: int.MaxValue),

            new(
                name: new IdentifierReference(FunctionName + "4"),
                returnTypeValue: Substitute.For<TypeValue>(),
                arguments: [],
                new BlockStatement([]),
                indexOfFirstDefaultArgument: int.MaxValue)];

        IFunctionWithUndefinedReturnStorage storage = new FunctionWithUndefinedReturnStorage();

        var removable = new FunctionSymbol(
            name: "key2",
            parameters: [],
            "undefined",
            isEmpty: false);

        storage.Save(new FunctionSymbol(
            name: "key1",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[0]);

        storage.Save(removable, declaration: declarations[1]);

        storage.Save(new FunctionSymbol(
            name: "key3",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[2]);

        storage.Save(new FunctionSymbol(
            name: "key4",
            parameters: [],
            "undefined",
            isEmpty: false), declaration: declarations[3]);

        storage.RemoveIfPresent(removable);

        Assert.Equal([declarations[0], declarations[2], declarations[3]], storage.Flush());
    }
}