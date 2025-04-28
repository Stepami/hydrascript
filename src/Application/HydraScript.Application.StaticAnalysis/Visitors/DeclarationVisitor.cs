using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using HydraScript.Domain.IR.Types;

namespace HydraScript.Application.StaticAnalysis.Visitors;

internal class DeclarationVisitor : VisitorNoReturnBase<IAbstractSyntaxTreeNode>,
    IVisitor<LexicalDeclaration>,
    IVisitor<FunctionDeclaration>
{
    private readonly IFunctionWithUndefinedReturnStorage _functionStorage;
    private readonly IMethodStorage _methodStorage;
    private readonly ISymbolTableStorage _symbolTables;
    private readonly IVisitor<TypeValue, Type> _typeBuilder;

    public DeclarationVisitor(
        IFunctionWithUndefinedReturnStorage functionStorage,
        IMethodStorage methodStorage,
        ISymbolTableStorage symbolTables,
        IVisitor<TypeValue, Type> typeBuilder)
    {
        _functionStorage = functionStorage;
        _methodStorage = methodStorage;
        _symbolTables = symbolTables;
        _typeBuilder = typeBuilder;
    }

    public override VisitUnit Visit(IAbstractSyntaxTreeNode visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        return default;
    }

    public VisitUnit Visit(LexicalDeclaration visitable)
    {
        for (var i = 0; i < visitable.Assignments.Count; i++)
        {
            var assignment = visitable.Assignments[i];
            if (_symbolTables[visitable.Scope].ContainsSymbol(new VariableSymbolId(assignment.Destination.Id)))
                throw new DeclarationAlreadyExists(assignment.Destination.Id);

            var destinationType = assignment.DestinationType?.Accept(
                _typeBuilder) ?? "undefined";

            if (destinationType == "undefined" &&
                assignment.Source is ImplicitLiteral or ArrayLiteral { Expressions.Count: 0 })
                throw visitable.ReadOnly
                    ? new ConstWithoutInitializer(assignment.Destination.Id)
                    : new CannotDefineType(assignment.Destination.Id.Segment);

            _symbolTables[visitable.Scope].AddSymbol(
                new VariableSymbol(
                    assignment.Destination.Id,
                    destinationType));
        }

        return default;
    }

    public VisitUnit Visit(FunctionDeclaration visitable)
    {
        var indexOfFirstDefaultArgument = visitable.Arguments
            .Select((x, i) => new { Argument = x, Index = i })
            .FirstOrDefault(pair => pair.Argument.Default)?.Index ?? -1;
        if (indexOfFirstDefaultArgument is not -1)
        {
            for (var i = indexOfFirstDefaultArgument; i < visitable.Arguments.Count; i++)
            {
                if (!visitable.Arguments[i].Default)
                    throw new NamedArgumentAfterDefaultValueArgument(
                        visitable.Segment,
                        function: visitable.Name,
                        visitable.Arguments[i]);
            }
        }

        var parameters = visitable.Arguments.Select(x =>
            new VariableSymbol(
                x.Name,
                x.TypeValue.Accept(_typeBuilder))).ToList();
        var functionSymbolId = new FunctionSymbolId(visitable.Name, parameters.Select(x => x.Type));
        visitable.ComputedFunctionAddress = functionSymbolId.ToString();
        if (_symbolTables[visitable.Parent.Scope].ContainsSymbol(functionSymbolId))
            throw new OverloadAlreadyExists(visitable.Name, functionSymbolId);

        for (var i = 0; i < parameters.Count; i++)
        {
            var arg = parameters[i];
            arg.Initialize();
            _symbolTables[visitable.Scope].AddSymbol(arg);
        }

        var functionSymbol = new FunctionSymbol(
            visitable.Name,
            parameters,
            visitable.ReturnTypeValue.Accept(_typeBuilder),
            visitable.IsEmpty);
        if (parameters is [{ Type: ObjectType objectType }, ..] &&
            visitable.Arguments is [{ TypeValue: TypeIdentValue }, ..])
        {
            _methodStorage.BindMethod(objectType, functionSymbol);
        }

        Type undefined = "undefined";
        if (functionSymbol.Type.Equals(undefined))
        {
            if (visitable.HasReturnStatement())
                _functionStorage.Save(functionSymbol, visitable);
            else
                functionSymbol.DefineReturnType("void");
        }

        _symbolTables[visitable.Parent.Scope].AddSymbol(functionSymbol);
        if (indexOfFirstDefaultArgument is not -1)
        {
            for (var i = indexOfFirstDefaultArgument; i < visitable.Arguments.Count; i++)
            {
                var overload = new FunctionSymbolId(visitable.Name, parameters[..i].Select(x => x.Type));
                _symbolTables[visitable.Parent.Scope].AddSymbol(functionSymbol, overload);
            }
        }
        return visitable.Statements.Accept(This);
    }
}