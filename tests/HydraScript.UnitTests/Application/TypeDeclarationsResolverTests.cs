using HydraScript.Application.StaticAnalysis.Impl;
using HydraScript.Application.StaticAnalysis.Visitors;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.IR.Impl;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using HydraScript.Domain.IR.Types;

namespace HydraScript.UnitTests.Application;

/// <summary>
/// Тесты <see cref="TypeDeclarationsResolver"/>
/// </summary>
public class TypeDeclarationsResolverTests
{
    /// <summary>
    /// https://github.com/Stepami/hydrascript/issues/231
    /// </summary>
    [Fact]
    public void Resolve_Issue231_Success()
    {
        // Arrange
        const string itemTypeName = "QueryStringParseResultItem";
        const string resultTypeName = "QueryStringParseResult";
        
        var scope = new Scope();
        var symbolTable = new SymbolTable();
        symbolTable.AddSymbol(new TypeSymbol(itemTypeName));
        symbolTable.AddSymbol(new TypeSymbol(resultTypeName));

        var symbolTables = new SymbolTableStorage();
        symbolTables.Init(scope, symbolTable);

        var resolver = new TypeDeclarationsResolver(
            new HydraScriptTypesService(),
            symbolTables,
            new TypeBuilder(symbolTables));

        foreach (var defaultType in resolver.TypesService.GetDefaultTypes())
        {
            symbolTable.AddSymbol(new TypeSymbol(defaultType));
        }

        var resultDeclaration = new TypeDeclaration(
            new IdentifierReference(resultTypeName),
            new ObjectTypeValue(
            [
                new PropertyTypeValue(
                    "result",
                    new ArrayTypeValue(
                        new TypeIdentValue(
                            new IdentifierReference(itemTypeName)))),
            ]));

        var itemDeclaration = new TypeDeclaration(
            new IdentifierReference(itemTypeName),
            new ObjectTypeValue(
            [
                new PropertyTypeValue("name", TypeIdentValue.String),
                new PropertyTypeValue("value", TypeIdentValue.String),
            ]));

        var root = new ScriptBody([resultDeclaration, itemDeclaration]);
        root.InitScope(scope);
        itemDeclaration.InitScope();
        resultDeclaration.InitScope();

        // Act
        resolver.Store(resultDeclaration);
        resolver.Store(itemDeclaration);

        resolver.Resolve();

        // Assert
        var itemType = symbolTable.FindSymbol(new TypeSymbolId(itemTypeName))?.Type;
        var resultType = symbolTable.FindSymbol(new TypeSymbolId(resultTypeName))?.Type as ObjectType;

        resultType.Should().NotBeNull();
        resultType["result"].Should().Be(itemType);
    }
}