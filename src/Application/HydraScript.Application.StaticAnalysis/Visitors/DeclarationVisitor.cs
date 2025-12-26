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
    private readonly IHydraScriptTypesService _typesService;
    private readonly IFunctionWithUndefinedReturnStorage _functionStorage;
    private readonly IMethodStorage _methodStorage;
    private readonly ISymbolTableStorage _symbolTables;
    private readonly IAmbiguousInvocationStorage _ambiguousInvocations;
    private readonly IVisitor<TypeValue, Type> _typeBuilder;
    private readonly IVisitor<FunctionDeclaration, ReturnAnalyzerResult> _returnAnalyzer;

    public DeclarationVisitor(
        IHydraScriptTypesService typesService,
        IFunctionWithUndefinedReturnStorage functionStorage,
        IMethodStorage methodStorage,
        ISymbolTableStorage symbolTables,
        IAmbiguousInvocationStorage ambiguousInvocations,
        IVisitor<TypeValue, Type> typeBuilder,
        IVisitor<FunctionDeclaration, ReturnAnalyzerResult> returnAnalyzer)
    {
        _typesService = typesService;
        _functionStorage = functionStorage;
        _methodStorage = methodStorage;
        _symbolTables = symbolTables;
        _ambiguousInvocations = ambiguousInvocations;
        _typeBuilder = typeBuilder;
        _returnAnalyzer = returnAnalyzer;
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
                _typeBuilder) ?? _typesService.Undefined;

            if (destinationType == _typesService.Undefined &&
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
        var returnAnalyzerResult = visitable.Accept(_returnAnalyzer);
        visitable.ReturnStatements = returnAnalyzerResult.ReturnStatements;
        visitable.AllCodePathsEndedWithReturn = returnAnalyzerResult.CodePathEndedWithReturn;

        var parentTable = _symbolTables[visitable.Parent.Scope];

        var parameters = new List<Type>();
        for (var i = 0; i < visitable.Arguments.Count; i++)
        {
            parameters.Add(visitable.Arguments[i].TypeValue.Accept(_typeBuilder));
            var arg = new VariableSymbol(visitable.Arguments[i].Name, parameters[i]);
            arg.Initialize();
            _symbolTables[visitable.Scope].AddSymbol(arg);
        }
        var functionSymbolId = new FunctionSymbolId(visitable.Name, parameters);
        _ambiguousInvocations.Clear(functionSymbolId);
        visitable.ComputedFunctionAddress = functionSymbolId.ToString();
        var functionSymbol = new FunctionSymbol(
            visitable.Name,
            parameters,
            visitable.ReturnTypeValue.Accept(_typeBuilder),
            visitable.IsEmpty);
        if (functionSymbolId.Equals(parentTable.FindSymbol(functionSymbolId)?.Id))
            throw new OverloadAlreadyExists(visitable.Name, functionSymbolId);

        if (parameters is [ObjectType methodOwner, ..] && visitable.Arguments is [{ TypeValue: TypeIdentValue }, ..])
            _methodStorage.BindMethod(methodOwner, functionSymbol, functionSymbolId);

        if (functionSymbol.Type.Equals(_typesService.Undefined))
        {
            if (visitable.HasReturnStatement)
                _functionStorage.Save(functionSymbol, visitable);
            else
                functionSymbol.DefineReturnType(_typesService.Void);
        }

        parentTable.AddSymbol(functionSymbol);
        for (var i = visitable.IndexOfFirstDefaultArgument; i < visitable.Arguments.Count; i++)
        {
            if (visitable.Arguments[i].Info.Type is ValueDtoType.Name)
                throw new NamedArgumentAfterDefaultValueArgument(
                    visitable.Segment,
                    function: visitable.Name,
                    visitable.Arguments[i]);

            var overload = new FunctionSymbolId(visitable.Name, parameters[..i]);
            var existing = parentTable.FindSymbol(overload);
            var functionToAdd = existing is not null && existing < functionSymbol
                ? existing
                : functionSymbol;
            parentTable.AddSymbol(functionToAdd, overload);
            if (parameters is [ObjectType overloadOwner, ..] && visitable.Arguments is [{ TypeValue: TypeIdentValue }, ..])
                _methodStorage.BindMethod(overloadOwner, functionToAdd, overload);

            if (existing is not null && !existing.Id.Equals(overload))
            {
                _ambiguousInvocations.WriteCandidate(overload, existing.Id);
                _ambiguousInvocations.WriteCandidate(overload, functionSymbolId);
            }
        }

        return visitable.Statements.Accept(This);
    }
}