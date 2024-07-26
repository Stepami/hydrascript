using HydraScript.Lib.IR.Ast;
using HydraScript.Lib.IR.Ast.Impl.Nodes;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using HydraScript.Lib.IR.Ast.Impl.Nodes.Statements;
using HydraScript.Lib.IR.CheckSemantics.Exceptions;
using HydraScript.Lib.IR.CheckSemantics.Types;
using HydraScript.Lib.IR.CheckSemantics.Variables.Symbols;
using HydraScript.Lib.IR.CheckSemantics.Visitors.Services;

namespace HydraScript.Lib.IR.CheckSemantics.Visitors;

public class SemanticChecker : VisitorBase<IAbstractSyntaxTreeNode, Type>,
    IVisitor<ScriptBody, Type>,
    IVisitor<WhileStatement, Type>,
    IVisitor<IfStatement, Type>,
    IVisitor<InsideStatementJump, Type>,
    IVisitor<ReturnStatement, Type>,
    IVisitor<ExpressionStatement, Type>,
    IVisitor<IdentifierReference, Type>,
    IVisitor<Literal, Type>,
    IVisitor<ImplicitLiteral, Type>,
    IVisitor<ArrayLiteral, Type>,
    IVisitor<ObjectLiteral, Type>,
    IVisitor<ConditionalExpression, Type>,
    IVisitor<BinaryExpression, Type>,
    IVisitor<UnaryExpression, Type>,
    IVisitor<LexicalDeclaration, Type>,
    IVisitor<AssignmentExpression, Type>,
    IVisitor<MemberExpression, Type>,
    IVisitor<IndexAccess, Type>,
    IVisitor<DotAccess, Type>,
    IVisitor<CastAsExpression, Type>,
    IVisitor<CallExpression, Type>,
    IVisitor<FunctionDeclaration, Type>,
    IVisitor<BlockStatement, Type>
{
    private readonly IDefaultValueForTypeCalculator _calculator;
    private readonly IFunctionWithUndefinedReturnStorage _functionStorage;
    private readonly IMethodStorage _methodStorage;
    private readonly IVisitor<TypeValue, Type> _typeBuilder = new TypeBuilder();

    public SemanticChecker(
        IDefaultValueForTypeCalculator calculator,
        IFunctionWithUndefinedReturnStorage functionStorage,
        IMethodStorage methodStorage)
    {
        _calculator = calculator;
        _functionStorage = functionStorage;
        _methodStorage = methodStorage;
    }

    public override Type DefaultVisit => "undefined";

    public Type Visit(ScriptBody visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);

        foreach (var funcDecl in _functionStorage.Flush())
            funcDecl.Accept(This);

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
            case InsideStatementJump.Break:
                if (!(visitable.ChildOf<IfStatement>() || visitable.ChildOf<WhileStatement>()))
                    throw new OutsideOfStatement(
                        visitable.Segment,
                        keyword: InsideStatementJump.Break,
                        statement: "if|while");
                break;
            case InsideStatementJump.Continue:
                if (!visitable.ChildOf<WhileStatement>())
                    throw new OutsideOfStatement(
                        visitable.Segment,
                        keyword: InsideStatementJump.Continue,
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
        var symbol = visitable.SymbolTable.FindSymbol<Symbol>(visitable.Name);
        return symbol?.Type ?? throw new UnknownIdentifierReference(visitable);
    }

    public Type Visit(Literal visitable) =>
        visitable.Type.Accept(_typeBuilder);

    public Type Visit(ImplicitLiteral visitable)
    {
        var type = visitable.Type.Accept(_typeBuilder);
        visitable.ComputedDefaultValue = _calculator.GetDefaultValueForType(type);
        return type;
    }

    public Type Visit(ArrayLiteral visitable)
    {
        if (visitable.Expressions.Count == 0)
            return new ArrayType(new Any());

        var type = visitable.First().Accept(This);
        if (visitable.Expressions.All(e => e.Accept(This).Equals(type)))
            return new ArrayType(type);

        throw new WrongArrayLiteralDeclaration(visitable.Segment, type);
    }

    public Type Visit(ObjectLiteral visitable)
    {
        var properties = visitable.Properties.Select(prop =>
        {
            var propType = prop.Expression.Accept(This);
            visitable.SymbolTable.AddSymbol(propType switch
            {
                ObjectType objectType => new ObjectSymbol(prop.Id, objectType),
                _ => new VariableSymbol(prop.Id, propType)
            });
            return new PropertyType(prop.Id, propType);
        });
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
                ? new List<ArrayType> { lArrType, rArrType }.First(x => x.Type is not Any)
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
        Type undefined = "undefined";

        for (var i = 0; i < visitable.Assignments.Count; i++)
        {
            var assignment = visitable.Assignments[i];
            var registeredSymbol = visitable.SymbolTable.FindSymbol<VariableSymbol>(
                assignment.Destination.Id);
            var sourceType = assignment.Source.Accept(This);
            if (sourceType.Equals(undefined))
                throw new CannotDefineType(assignment.Source.Segment);
            if (!registeredSymbol!.Type.Equals(undefined) && !registeredSymbol.Type.Equals(sourceType))
                throw new IncompatibleTypesOfOperands(
                    assignment.Segment,
                    left: registeredSymbol.Type,
                    right: sourceType);

            var actualType = registeredSymbol.Type.Equals(undefined)
                ? sourceType
                : registeredSymbol.Type;
            var actualSymbol = actualType switch
            {
                ObjectType objectType => new ObjectSymbol(registeredSymbol.Id, objectType, visitable.ReadOnly),
                _ => new VariableSymbol(registeredSymbol.Id, actualType, visitable.ReadOnly)
            };
            visitable.SymbolTable.AddSymbol(actualSymbol);
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

        var symbol =
            visitable.SymbolTable.FindSymbol<VariableSymbol>(
                visitable.Destination.Id) ??
            throw new UnknownIdentifierReference(visitable.Destination.Id);

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
        visitable.ComputedIdType = idType;
        return visitable.Empty() ? idType : visitable.AccessChain?.Accept(This) ?? "undefined";
    }

    public Type Visit(IndexAccess visitable)
    {
        var prevType =
            visitable.Prev?.ComputedType
            ?? (visitable.Parent as MemberExpression)!.ComputedIdType;

        if (prevType is not ArrayType arrayType)
            throw new NonAccessibleType(prevType);

        Type number = "number";
        var indexType = visitable.Index.Accept(This);
        if (!indexType.Equals(number))
            throw new ArrayAccessException(visitable.Segment, indexType);

        var elemType = arrayType.Type;
        visitable.ComputedType = elemType;
        return visitable.HasNext() ? visitable.Next?.Accept(This) ?? "undefined" : elemType;
    }

    public Type Visit(DotAccess visitable)
    {
        var prevType =
            visitable.Prev?.ComputedType
            ?? (visitable.Parent as MemberExpression)!.ComputedIdType;

        if (prevType is not ObjectType objectType)
            throw new NonAccessibleType(prevType);

        var fieldType = objectType[visitable.Property];
        var hasMethod = objectType.HasMethod(visitable.Property);
        if (fieldType is null)
            return hasMethod
                ? objectType
                : throw new ObjectAccessException(visitable.Segment, objectType, visitable.Property);
        visitable.ComputedType = fieldType;
        return visitable.HasNext() ? visitable.Next?.Accept(This) ?? "undefined" : fieldType;
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
        FunctionSymbol functionSymbol;
        var methodCall = !visitable.Member.Empty();

        if (methodCall)
        {
            var objectType = (ObjectType)visitable.Member.Accept(This);
            var availableMethods = _methodStorage.GetAvailableMethods(objectType);
            functionSymbol = availableMethods[objectType.LastAccessedMethod];
        }
        else
        {
            var symbol =
                visitable.SymbolTable.FindSymbol<Symbol>(visitable.Id)
                ?? throw new UnknownIdentifierReference(visitable.Id);
            functionSymbol =
                symbol as FunctionSymbol
                ?? throw new SymbolIsNotCallable(symbol.Id, visitable.Id.Segment);
        }

        var functionReturnType = functionSymbol.Type;

        if (functionSymbol.Parameters.Count != visitable.Parameters.Count + (methodCall ? 1 : 0))
            throw new WrongNumberOfArguments(
                visitable.Segment,
                expected: functionSymbol.Parameters.Count,
                actual: visitable.Parameters.Count);

        visitable.Parameters.Zip(functionSymbol.Parameters.ToArray()[(methodCall ? 1 : 0)..])
            .ToList().ForEach(pair =>
            {
                var (expr, expected) = pair;
                var actualType = expr.Accept(This);
                if (!actualType.Equals(expected.Type))
                    throw new WrongTypeOfArgument(expr.Segment, expected.Type, actualType);
            });

        Type undefined = "undefined";
        if (functionSymbol.Type.Equals(undefined))
        {
            var declaration = _functionStorage.Get(functionSymbol);
            functionReturnType = declaration.Accept(This);
        }

        return functionReturnType;
    }

    public Type Visit(FunctionDeclaration visitable)
    {
        var symbol = visitable.SymbolTable.FindSymbol<FunctionSymbol>(visitable.Name)!;
        _functionStorage.RemoveIfPresent(symbol);
        visitable.Statements.Accept(This);

        var returnStatements = visitable.ReturnStatements
            .Select(x => new
            {
                Statement = x,
                Type = x.Accept(This)
            });
        Type undefined = "undefined";
        if (symbol.Type.Equals(undefined))
        {
            var returnStatementTypes = returnStatements
                .GroupBy(x => x.Type)
                .Select(x => x.Key)
                .ToList();
            if (returnStatementTypes.Count > 1)
                throw new CannotDefineType(visitable.Segment);
            symbol.DefineReturnType(returnStatementTypes.ElementAtOrDefault(0) ?? "void");
        }
        else
        {
            var wrongReturn = returnStatements
                .FirstOrDefault(x => !x.Type.Equals(symbol.Type));
            if (wrongReturn is not null)
                throw new WrongReturnType(
                    wrongReturn.Statement.Segment,
                    expected: symbol.Type,
                    actual: wrongReturn.Type);
        }

        Type @void = "void";
        var hasReturnStatement = visitable.HasReturnStatement();
        if (!symbol.Type.Equals(@void) && !hasReturnStatement)
            throw new FunctionWithoutReturnStatement(visitable.Segment);

        return symbol.Type;
    }

    public Type Visit(BlockStatement visitable)
    {
        for (var i = 0; i < visitable.Count; i++)
            visitable[i].Accept(This);
        return "undefined";
    }
}