using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Visitor.NET;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors;

public class DeclarationVisitor : 
    IVisitor<AbstractSyntaxTreeNode>,
    IVisitor<LexicalDeclaration>,
    IVisitor<FunctionDeclaration>
{
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
            var destinationType = assignment.DestinationType?.BuildType(
                assignment.SymbolTable) ?? "undefined";
            visitable.SymbolTable.AddSymbol(
                new VariableSymbol(
                    assignment.Destination.Id,
                    destinationType,
                    visitable.Readonly));
        }

        return default;
    }

    public Unit Visit(FunctionDeclaration visitable)
    {
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
                arguments: parameters.Select(x => x.Type)));

        visitable.Parent.SymbolTable.AddSymbol(functionSymbol);
        return default;
    }
}