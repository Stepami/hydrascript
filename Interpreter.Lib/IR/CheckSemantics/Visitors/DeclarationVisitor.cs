using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors.Services;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors;

public class DeclarationVisitor :
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<LexicalDeclaration>,
    IVisitor<FunctionDeclaration>
{
    private readonly IFunctionWithUndefinedReturnStorage _storage;

    public DeclarationVisitor(IFunctionWithUndefinedReturnStorage storage) =>
        _storage = storage;

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
        if (visitable.SymbolTable.ContainsSymbol(visitable.Name))
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
            new FunctionType(
                visitable.ReturnTypeValue.BuildType(visitable.Parent.SymbolTable),
                arguments: parameters.Select(x => x.Type)),
            isEmpty: !visitable.Statements.Any());

        Type undefined = "undefined";
        if (functionSymbol.Type.ReturnType.Equals(undefined))
            _storage.Save(functionSymbol, visitable);

        visitable.Parent.SymbolTable.AddSymbol(functionSymbol);
        return visitable.Statements.Accept(this);
    }
}