using HydraScript.Application.StaticAnalysis.Exceptions;
using HydraScript.Domain.Constants;
using HydraScript.Domain.FrontEnd.Parser;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.AccessExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.ComplexLiterals;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Domain.FrontEnd.Parser.Impl.Ast.Nodes.Statements;
using HydraScript.Domain.IR.Impl.Symbols;
using HydraScript.Domain.IR.Impl.Symbols.Ids;
using HydraScript.Domain.IR.Types;
using ZLinq;

namespace HydraScript.Application.StaticAnalysis.Visitors;

internal class SemanticChecker : VisitorBase<IAbstractSyntaxTreeNode, Type>,
    IVisitor<ScriptBody, Type>,
    IVisitor<WhileStatement, Type>,
    IVisitor<IfStatement, Type>,
    IVisitor<InsideStatementJump, Type>,
    IVisitor<ReturnStatement, Type>,
    IVisitor<ExpressionStatement, Type>,
    IVisitor<IdentifierReference, Type>,
    IVisitor<EnvVarReference, Type>,
    IVisitor<Literal, Type>,
    IVisitor<ImplicitLiteral, Type>,
    IVisitor<ArrayLiteral, ArrayType>,
    IVisitor<ObjectLiteral, ObjectType>,
    IVisitor<ConditionalExpression, Type>,
    IVisitor<BinaryExpression, Type>,
    IVisitor<UnaryExpression, Type>,
    IVisitor<LexicalDeclaration, Type>,
    IVisitor<AssignmentExpression, Type>,
    IVisitor<MemberExpression, Type>,
    IVisitor<IndexAccess, Type>,
    IVisitor<DotAccess, Type>,
    IVisitor<CastAsExpression, Type>,
    IVisitor<WithExpression, ObjectType>,
    IVisitor<CallExpression, Type>,
    IVisitor<FunctionDeclaration, Type>,
    IVisitor<BlockStatement, Type>,
    IVisitor<PrintStatement, Type>
{
    private readonly IDefaultValueForTypeCalculator _calculator;
    private readonly IFunctionWithUndefinedReturnStorage _functionStorage;
    private readonly IMethodStorage _methodStorage;
    private readonly ISymbolTableStorage _symbolTables;
    private readonly IComputedTypesStorage _computedTypes;
    private readonly IAmbiguousInvocationStorage _ambiguousInvocations;
    private readonly IVisitor<TypeValue, Type> _typeBuilder;

    public SemanticChecker(
        IDefaultValueForTypeCalculator calculator,
        IFunctionWithUndefinedReturnStorage functionStorage,
        IMethodStorage methodStorage,
        ISymbolTableStorage symbolTables,
        IComputedTypesStorage computedTypes,
        IAmbiguousInvocationStorage ambiguousInvocations,
        IVisitor<TypeValue, Type> typeBuilder)
    {
        _calculator = calculator;
        _functionStorage = functionStorage;
        _methodStorage = methodStorage;
        _symbolTables = symbolTables;
        _computedTypes = computedTypes;
        _ambiguousInvocations = ambiguousInvocations;
        _typeBuilder = typeBuilder;
    }

    public override Type Visit(IAbstractSyntaxTreeNode visitable) => "undefined";

    public Type Visit(ScriptBody visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        foreach (var funcDecl in _functionStorage.Flush())
            funcDecl.Accept(This);

        _methodStorage.Clear();
        _symbolTables.Clear();
        _computedTypes.Clear();
        _ambiguousInvocations.Clear();
        
        return "undefined";
    }

    public Type Visit(WhileStatement visitable)
    {
        var condType = visitable.Condition.Accept(This);
        Type boolean = "boolean";
        if (!condType.Equals(boolean))
            throw new NotBooleanTestExpression(visitable.Segment, condType);

        visitable.Statement.Accept(This);

        return "undefined";
    }

    public Type Visit(IfStatement visitable)
    {
        var testType = visitable.Test.Accept(This);
        Type boolean = "boolean";
        if (!testType.Equals(boolean))
            throw new NotBooleanTestExpression(visitable.Segment, testType);

        visitable.Then.Accept(This);
        visitable.Else?.Accept(This);

        return "undefined";
    }

    public Type Visit(InsideStatementJump visitable)
    {
        switch (visitable.Keyword)
        {
            case InsideStatementJumpKeyword.Break:
                if (!(visitable.ChildOf<IfStatement>() || visitable.ChildOf<WhileStatement>()))
                    throw new OutsideOfStatement(
                        visitable.Segment,
                        visitable.Keyword,
                        statement: "if|while");
                break;
            case InsideStatementJumpKeyword.Continue:
                if (!visitable.ChildOf<WhileStatement>())
                    throw new OutsideOfStatement(
                        visitable.Segment,
                        visitable.Keyword,
                        statement: "while");
                break;
        }

        return "undefined";
    }

    public Type Visit(ReturnStatement visitable)
    {
        if (!visitable.ChildOf<FunctionDeclaration>())
            throw new ReturnOutsideFunction(visitable.Segment);

        return visitable.Expression?.Accept(This) ?? "void";
    }

    public Type Visit(ExpressionStatement visitable) =>
        visitable.Expression.Accept(This);

    public Type Visit(IdentifierReference visitable)
    {
        var symbol = _symbolTables[visitable.Scope].FindSymbol(new VariableSymbolId(visitable.Name));
        if (symbol is { Initialized: false })
            throw new AccessBeforeInitialization(visitable);
        return symbol?.Type ?? throw new UnknownIdentifierReference(visitable);
    }

    public Type Visit(EnvVarReference visitable) => "string";

    public Type Visit(Literal visitable) =>
        visitable.Type.Accept(_typeBuilder);

    public Type Visit(ImplicitLiteral visitable)
    {
        var type = visitable.Type.Accept(_typeBuilder);
        if (!visitable.IsDefined)
        {
            var definedValue = _calculator.GetDefaultValueForType(type);
            visitable.SetValue(definedValue);
        }

        return type;
    }

    public ArrayType Visit(ArrayLiteral visitable)
    {
        if (visitable.Expressions.Count == 0)
            return new ArrayType(new Any());

        var type = visitable[0].Accept(This);
        if (visitable.Expressions.All(e => e.Accept(This).Equals(type)))
            return new ArrayType(type);

        throw new WrongArrayLiteralDeclaration(visitable.Segment, type);
    }

    public ObjectType Visit(ObjectLiteral visitable)
    {
        var properties = visitable.Properties.AsValueEnumerable().Select(prop =>
        {
            var propType = prop.Expression.Accept(This);

            if (propType is NullType &&
                (visitable.ChildOf<AssignmentExpression>(
                     x => x.ChildOf<LexicalDeclaration>() && x.DestinationType is null) ||
                 visitable.ChildOf<ReturnStatement>(x => x.ChildOf<FunctionDeclaration>(
                     decl => decl.ReturnTypeValue is TypeIdentValue { TypeId.Name: "undefined" }))))
                throw new CannotAssignNullWhenUndefined(prop.Segment);

            var propSymbol = propType switch
            {
                ObjectType objectType => new ObjectSymbol(prop.Id, objectType),
                _ => new VariableSymbol(prop.Id, propType)
            };
            propSymbol.Initialize();
            _symbolTables[visitable.Scope].AddSymbol(propSymbol);
            return new PropertyType(prop.Id, propType);
        }).OrderBy(x => x.Id).ToDictionary(
            x => x.Id,
            x => x.Type);
        var objectLiteralType = new ObjectType(properties);
        return objectLiteralType;
    }

    public Type Visit(ConditionalExpression visitable)
    {
        var tType = visitable.Test.Accept(This);
        Type boolean = "boolean";
        if (!tType.Equals(boolean))
            throw new NotBooleanTestExpression(visitable.Test.Segment, tType);

        var cType = visitable.Consequent.Accept(This);
        var aType = visitable.Alternate.Accept(This);
        if (cType.Equals(aType))
            return cType;

        throw new WrongConditionalTypes(
            segment: visitable.Segment,
            cSegment: visitable.Consequent.Segment,
            cType,
            aSegment: visitable.Alternate.Segment,
            aType);
    }

    public Type Visit(BinaryExpression visitable)
    {
        var lType = visitable.Left.Accept(This);
        var rType = visitable.Right.Accept(This);

        if (visitable.Operator != "::" && !lType.Equals(rType))
            throw new IncompatibleTypesOfOperands(
                visitable.Segment,
                left: lType,
                right: rType);

        Type number = "number";
        Type @string = "string";
        Type boolean = "boolean";

        return visitable.Operator switch
        {
            "+" when lType.Equals(number) => number,
            "+" when lType.Equals(@string) => @string,
            "+" => throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "-" or "*" or "/" or "%" => lType.Equals(number)
                ? number
                : throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "||" or "&&" => lType.Equals(boolean)
                ? boolean
                : throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "==" or "!=" => boolean,
            ">" or ">=" or "<" or "<=" => lType.Equals(number)
                ? boolean
                : throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "++" when lType is ArrayType { Type: Any } && rType is ArrayType { Type: Any } =>
                throw new CannotDefineType(visitable.Segment),
            "++" => lType is ArrayType lArrType && rType is ArrayType rArrType
                ? lArrType.Type is not Any ? lArrType : rArrType.Type is not Any ? rArrType : throw new CannotDefineType(visitable.Segment)
                : throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "::" when lType is not ArrayType =>
                throw new UnsupportedOperation(visitable.Segment, lType, visitable.Operator),
            "::" => rType.Equals(number) ? "void" : throw new ArrayAccessException(visitable.Segment, rType),
            _ => "undefined"
        };
    }

    public Type Visit(UnaryExpression visitable)
    {
        var eType = visitable.Expression.Accept(This);

        Type number = "number";
        Type boolean = "boolean";

        return visitable.Operator switch
        {
            "-" when eType.Equals(number) => number,
            "!" when eType.Equals(boolean) => boolean,
            "~" when eType is ArrayType => number,
            _ => throw new UnsupportedOperation(visitable.Segment, eType, visitable.Operator)
        };
    }

    public Type Visit(LexicalDeclaration visitable)
    {
        Type undefined = "undefined", @void = "void";

        for (var i = 0; i < visitable.Assignments.Count; i++)
        {
            var assignment = visitable.Assignments[i];
            var registeredSymbol = _symbolTables[visitable.Scope].FindSymbol(new VariableSymbolId(assignment.Destination.Id))!;
            var sourceType = assignment.Source.Accept(This);

            if (sourceType.Equals(undefined))
                throw new CannotDefineType(assignment.Source.Segment);
            if (sourceType.Equals(@void))
                throw new CannotAssignVoid(assignment.Source.Segment);
            if (!registeredSymbol.Type.Equals(undefined) && !registeredSymbol.Type.Equals(sourceType))
                throw new IncompatibleTypesOfOperands(
                    assignment.Segment,
                    left: registeredSymbol.Type,
                    right: sourceType);
            if (sourceType is NullType && registeredSymbol.Type.Equals(undefined))
                throw new CannotAssignNullWhenUndefined(assignment.Segment);

            var actualType = registeredSymbol.Type.Equals(undefined)
                ? sourceType
                : registeredSymbol.Type;
            var actualSymbol = actualType switch
            {
                ObjectType objectType => new ObjectSymbol(registeredSymbol.Name, objectType, visitable.ReadOnly),
                _ => new VariableSymbol(registeredSymbol.Name, actualType, visitable.ReadOnly)
            };
            actualSymbol.Initialize();
            _symbolTables[visitable.Scope].AddSymbol(actualSymbol);
        }

        return undefined;
    }

    public Type Visit(AssignmentExpression visitable)
    {
        if (visitable.Destination is CallExpression)
            throw new WrongAssignmentTarget(visitable.Destination);

        var sourceType = visitable.Source.Accept(This);
        if (!visitable.Destination.Empty())
        {
            var destinationType = visitable.Destination.Accept(This);
            if (!destinationType.Equals(sourceType))
                throw new IncompatibleTypesOfOperands(
                    visitable.Segment,
                    left: destinationType,
                    right: sourceType);
            return destinationType;
        }

        // здесь может быть переменная программы, а может быть переменная среды
        var symbol = visitable.Destination.Id.ToValueDto().Type is ValueDtoType.Name
            ? _symbolTables[visitable.Scope].FindSymbol(new VariableSymbolId(visitable.Destination.Id)) ??
              throw new UnknownIdentifierReference(visitable.Destination.Id)
            : new VariableSymbol(visitable.Destination.Id, "string");

        if (symbol.ReadOnly)
            throw new AssignmentToConst(visitable.Destination.Id);

        if (!sourceType.Equals(symbol.Type))
            throw new IncompatibleTypesOfOperands(
                visitable.Segment,
                left: symbol.Type,
                right: sourceType);

        return symbol.Type;
    }

    public Type Visit(MemberExpression visitable)
    {
        var idType = visitable.Id.Accept(This);
        visitable.ComputedIdTypeGuid = _computedTypes.Save(idType);
        return visitable.Empty() ? idType : visitable.AccessChain?.Accept(This) ?? "undefined";
    }

    public Type Visit(IndexAccess visitable)
    {
        var prevTypeGuid =
            visitable.Prev?.ComputedTypeGuid
            ?? (visitable.Parent as MemberExpression)!.ComputedIdTypeGuid;
        var prevType = _computedTypes.Get(prevTypeGuid);

        if (prevType is not ArrayType arrayType)
            throw new NonAccessibleType(prevType);

        Type number = "number";
        var indexType = visitable.Index.Accept(This);
        if (!indexType.Equals(number))
            throw new ArrayAccessException(visitable.Segment, indexType);

        var elemType = arrayType.Type;
        visitable.ComputedTypeGuid = _computedTypes.Save(elemType);
        return visitable.HasNext() ? visitable.Next?.Accept(This) ?? "undefined" : elemType;
    }

    public Type Visit(DotAccess visitable)
    {
        var prevTypeGuid =
            visitable.Prev?.ComputedTypeGuid
            ?? (visitable.Parent as MemberExpression)!.ComputedIdTypeGuid;
        var prevType = _computedTypes.Get(prevTypeGuid);

        if (prevType is not ObjectType objectType)
            throw new NonAccessibleType(prevType);

        var fieldType = objectType[visitable.Property];
        var hasMethod = objectType.HasMethod(visitable.Property);
        if (fieldType is null)
            return hasMethod
                ? objectType
                : throw new ObjectAccessException(visitable.Segment, objectType, visitable.Property);
        visitable.ComputedTypeGuid = _computedTypes.Save(fieldType);
        return visitable.HasNext() ? visitable.Next?.Accept(This) ?? "undefined" : fieldType;
    }

    public ObjectType Visit(WithExpression visitable)
    {
        var exprType = visitable.Expression.Accept(This);

        if (exprType is not ObjectType supersetObjectType)
            throw new UnsupportedOperation(visitable.Segment, exprType, "with");

        IVisitor<ObjectLiteral, ObjectType> objectLiteralVisitor = this;
        var subsetObjectType = visitable.ObjectLiteral.Accept(objectLiteralVisitor);

        if (!supersetObjectType.IsSubsetOf(subsetObjectType))
            throw new IncompatibleTypesOfOperands(
                visitable.Segment,
                left: supersetObjectType,
                right: subsetObjectType);

        visitable.ComputedCopiedProperties = supersetObjectType.CalculateDifference(subsetObjectType);

        return supersetObjectType;
    }

    public Type Visit(CastAsExpression visitable)
    {
        Type undefined = "undefined";
        var exprType = visitable.Expression.Accept(This);

        if (exprType.Equals(undefined))
            throw new CannotDefineType(visitable.Expression.Segment);

        return visitable.Cast.Accept(_typeBuilder) == "string"
            ? "string"
            : throw new NotSupportedException("Other types but 'string' have not been supported for casting yet");
    }

    public Type Visit(CallExpression visitable)
    {
        var parameters = visitable.Parameters.Select(expr => expr.Accept(This)).ToList();
        FunctionSymbol functionSymbol;
        var methodCall = !visitable.Member.Empty();

        if (methodCall)
        {
            var objectType = (ObjectType)visitable.Member.Accept(This);
            var availableMethods = _methodStorage.GetAvailableMethods(objectType);
            var methodKey = new FunctionSymbolId(objectType.LastAccessedMethodName, [objectType, ..parameters]);
            _ambiguousInvocations.CheckCandidatesAndThrow(visitable.Segment, methodKey);
            functionSymbol =
                availableMethods.GetValueOrDefault(methodKey)
                ?? throw new UnknownFunctionOverload(visitable.Id, methodKey);
        }
        else
        {
            var functionKey = new FunctionSymbolId(visitable.Id, parameters);
            _ambiguousInvocations.CheckCandidatesAndThrow(visitable.Segment, functionKey);
            functionSymbol =
                _symbolTables[visitable.Scope].FindSymbol(functionKey)
                ?? throw new UnknownFunctionOverload(visitable.Id, functionKey);
        }

        visitable.ComputedFunctionAddress = functionSymbol.Id.ToString();
        visitable.IsEmptyCall = functionSymbol.IsEmpty;
        var functionReturnType = functionSymbol.Type;

        visitable.Parameters.AsValueEnumerable()
            .Zip(parameters).Zip(functionSymbol.Parameters.AsValueEnumerable().Skip(methodCall ? 1 : 0))
            .ToList().ForEach(pair =>
            {
                var ((expr, actualType), expectedType) = pair;
                if (!actualType.Equals(expectedType))
                    throw new WrongTypeOfArgument(expr.Segment, expectedType, actualType);
            });

        Type undefined = "undefined";
        if (functionSymbol.Type.Equals(undefined))
        {
            var declaration = _functionStorage.Get(functionSymbol);
            functionReturnType = declaration.Accept(This);
        }

        Type @void = "void";
        if (!functionReturnType.Equals(@void))
            visitable.HasReturnValue = true;
        return functionReturnType;
    }

    public Type Visit(FunctionDeclaration visitable)
    {
        var parameters = visitable.Arguments.Select(x => x.TypeValue.Accept(_typeBuilder)).ToList();
        var symbol = _symbolTables[visitable.Scope].FindSymbol(new FunctionSymbolId(visitable.Name, parameters))!;
        _functionStorage.RemoveIfPresent(symbol);
        visitable.Statements.Accept(This);

        Type undefined = "undefined";
        HashSet<Type> returnTypes = [];
        for (var i = 0; i < visitable.ReturnStatements.Count; i++)
        {
            var returnStatementType = visitable.ReturnStatements[i].Accept(This);
            returnTypes.Add(returnStatementType);
            if (returnTypes.Count > 1 && symbol.Type.Equals(undefined))
                throw new CannotDefineType(visitable.Segment);
            if (!symbol.Type.Equals(undefined) && !symbol.Type.Equals(returnStatementType))
                throw new WrongReturnType(
                    visitable.ReturnStatements[i].Segment,
                    expected: symbol.Type,
                    actual: returnStatementType);
        }

        if (symbol.Type.Equals(undefined))
            symbol.DefineReturnType(returnTypes.Single());

        Type @void = "void";
        if (!symbol.Type.Equals(@void) && !visitable.AllCodePathsEndedWithReturn)
            throw new FunctionWithoutReturnStatement(visitable.Segment);

        if (symbol.Type is NullType)
            throw new CannotAssignNullWhenUndefined(visitable.Segment);

        return symbol.Type;
    }

    public Type Visit(BlockStatement visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return "undefined";
    }

    public Type Visit(PrintStatement visitable)
    {
        visitable.Expression.Accept(This);
        return "undefined";
    }
}