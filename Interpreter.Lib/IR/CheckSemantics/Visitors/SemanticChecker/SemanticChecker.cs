using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;
using Interpreter.Lib.IR.CheckSemantics.Exceptions;
using Interpreter.Lib.IR.CheckSemantics.Types;
using Interpreter.Lib.IR.CheckSemantics.Variables.Symbols;
using Interpreter.Lib.IR.CheckSemantics.Visitors.SemanticChecker.Service;
using Visitor.NET;

namespace Interpreter.Lib.IR.CheckSemantics.Visitors.SemanticChecker;

public class SemanticChecker :
    IVisitor<ScriptBody, Type>,
    IVisitor<WhileStatement, Type>,
    IVisitor<IfStatement, Type>,
    IVisitor<InsideStatementJump, Type>,
    IVisitor<ReturnStatement, Type>,
    IVisitor<IdentifierReference, Type>,
    IVisitor<ImplicitLiteral, Type>,
    IVisitor<ArrayLiteral, Type>,
    IVisitor<ConditionalExpression, Type>
{
    private readonly IDefaultValueForTypeCalculator _calculator;

    public SemanticChecker(IDefaultValueForTypeCalculator calculator) =>
        _calculator = calculator;


    public Type Visit(ScriptBody visitable)
    {
        foreach (var statementListItem in visitable.StatementList)
            statementListItem.Accept(this);

        return "undefined";
    }

    public Type Visit(WhileStatement visitable)
    {
        var condType = visitable.Condition.Accept(this);
        Type boolean = "boolean";
        if (!condType.Equals(boolean))
        {
            throw new NotBooleanTestExpression(visitable.Segment, condType);
        }

        return default;
    }
    
    public Type Visit(IfStatement visitable)
    {
        var testType = visitable.Test.Accept(this);
        Type boolean = "boolean";
        if (!testType.Equals(boolean))
        {
            throw new NotBooleanTestExpression(visitable.Segment, testType);
        }
        
        return default;
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

        return default;
    }

    public Type Visit(ReturnStatement visitable)
    {
        if (!visitable.ChildOf<FunctionDeclaration>())
        {
            throw new ReturnOutsideFunction(visitable.Segment);
        }

        return visitable.Expression?.Accept(this) ?? "void";
    }

    public Type Visit(IdentifierReference visitable)
    {
        if (visitable.ChildOf<DotAccess>())
            return "undefined";

        var symbol = visitable.SymbolTable.FindSymbol<Symbol>(visitable.Name);
        return symbol switch
        {
            { State: SymbolState.Declared } => throw new NotSupportedException(), // todo symbol not init error 
            { State: SymbolState.Initialized } => symbol.Type,
            null => throw new UnknownIdentifierReference(visitable),
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }

    public Type Visit(ImplicitLiteral visitable)
    {
        var type = visitable.TypeValue.BuildType(visitable.Parent.SymbolTable);
        visitable.ComputedDefaultValue = _calculator.GetDefaultValueForType(type);
        return type;
    }

    public Type Visit(ArrayLiteral visitable)
    {
        if (!visitable.Expressions.Any())
            return "undefined";

        var type = visitable.First().Accept(this);
        if (visitable.Expressions.All(e => e.Accept(this).Equals(type)))
            return new ArrayType(type);

        throw new WrongArrayLiteralDeclaration(visitable.Segment, type);
    }

    public Type Visit(ConditionalExpression visitable)
    {
        var tType = visitable.Test.Accept(this);
        Type boolean = "boolean";
        if (!tType.Equals(boolean))
            throw new NotBooleanTestExpression(visitable.Test.Segment, tType);

        var cType = visitable.Consequent.Accept(this);
        var aType = visitable.Alternate.Accept(this);
        if (cType.Equals(aType))
            return cType;

        throw new WrongConditionalTypes(
            cSegment: visitable.Consequent.Segment,
            cType,
            aSegment: visitable.Alternate.Segment,
            aType);
    }
}