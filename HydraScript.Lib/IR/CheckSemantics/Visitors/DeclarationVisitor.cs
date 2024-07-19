using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class DeclarationVisitor :
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<LexicalDeclaration>,
    IVisitor<FunctionDeclaration>
{
    private readonly IFunctionWithUndefinedReturnStorage _functionStorage;
    private readonly IMethodStorage _methodStorage;

    public DeclarationVisitor(
        IFunctionWithUndefinedReturnStorage functionStorage,
        IMethodStorage methodStorage)
    {
        _functionStorage = functionStorage;
        _methodStorage = methodStorage;
    }

    public Unit Visit(AbstractSyntaxTreeNode visitable)
    {
        foreach (var child in visitable)
            child.Accept(this);

        return default;
    }

    public Unit Visit(LexicalDeclaration visitable)
    {
        foreach (var assignment in visitable.Assignments)
        {
            if (visitable.SymbolTable.ContainsSymbol(assignment.Destination.Id))
                throw new DeclarationAlreadyExists(assignment.Destination.Id);

            var destinationType = assignment.DestinationType?.BuildType(
                assignment.SymbolTable) ?? "undefined";

            if (destinationType == "undefined" &&
                assignment.Source is ImplicitLiteral or ArrayLiteral { Expressions.Count: 0 })
                throw visitable.ReadOnly
                    ? new ConstWithoutInitializer(assignment.Destination.Id)
                    : new CannotDefineType(assignment.Destination.Id.Segment);

            visitable.SymbolTable.AddSymbol(
                new VariableSymbol(
                    assignment.Destination.Id,
                    destinationType));
        }

        return default;
    }

    public Unit Visit(FunctionDeclaration visitable)
    {
        if (visitable.Parent.SymbolTable.ContainsSymbol(visitable.Name))
            throw new DeclarationAlreadyExists(visitable.Name);

        var parameters = visitable.Arguments.Select(x =>
        {
            var arg = new VariableSymbol(
                id: x.Key,
                x.TypeValue.BuildType(visitable.Parent.SymbolTable));
            visitable.SymbolTable.AddSymbol(arg);
            return arg;
        }).ToList();

        var functionSymbol = new FunctionSymbol(
            visitable.Name,
            parameters,
            visitable.ReturnTypeValue.BuildType(visitable.Parent.SymbolTable),
            isEmpty: !visitable.Statements.Any());
        if (parameters is [{ Type: ObjectType objectType }, ..] &&
            visitable.Arguments is [{ TypeValue: TypeIdentValue }, ..])
        {
            _methodStorage.BindMethod(objectType, functionSymbol);
        }

        Type undefined = "undefined";
        if (functionSymbol.Type.Equals(undefined))
            _functionStorage.Save(functionSymbol, visitable);

        visitable.Parent.SymbolTable.AddSymbol(functionSymbol);
        return visitable.Statements.Accept(this);
    }
}