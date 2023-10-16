using System.Globalization;
using System.Text.RegularExpressions;
using Interpreter.Lib.FrontEnd.GetTokens;
using Interpreter.Lib.FrontEnd.GetTokens.Data;
using Interpreter.Lib.IR.Ast;
using Interpreter.Lib.IR.Ast.Impl;
using Interpreter.Lib.IR.Ast.Impl.Nodes;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Declarations.AfterTypesAreLoaded;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.AccessExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.ComplexLiterals;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Expressions.PrimaryExpressions;
using Interpreter.Lib.IR.Ast.Impl.Nodes.Statements;

namespace Interpreter.Lib.FrontEnd.TopDownParse.Impl;

public class Parser : IParser
{
    private TokensStream _tokens;
    private readonly ILexer _lexer;

    public Parser(ILexer lexer) => 
        _lexer = lexer;
    
    public IAbstractSyntaxTree TopDownParse(string text)
    {
        _tokens = _lexer.GetTokens(text);
            
        var root = Script();
        Expect("EOP");
        return new AbstractSyntaxTree(root);
    }

    private Token Expect(string expectedTag, string expectedValue = null)
    {
        var current = _tokens.Current;

        if (!CurrentIs(expectedTag))
            throw new ParserException(_tokens.Current!.Segment, expectedTag, _tokens.Current);
        if (_tokens.Current!.Value != (expectedValue ?? _tokens.Current.Value))
            throw new ParserException(_tokens.Current.Segment, expectedValue, _tokens.Current);

        _tokens.MoveNext();
        return current;
    }

    private bool CurrentIs(string tag) =>
        _tokens.Current!.Type == _lexer.Structure.FindByTag(tag);

    private bool CurrentIsLiteral() =>
        CurrentIs("NullLiteral") ||
        CurrentIs("IntegerLiteral") ||
        CurrentIs("FloatLiteral") ||
        CurrentIs("StringLiteral") ||
        CurrentIs("BooleanLiteral");

    private bool CurrentIsKeyword(string keyword) =>
        CurrentIs("Keyword") &&
        _tokens.Current!.Value == keyword;

    private bool CurrentIsOperator(string @operator) =>
        CurrentIs("Operator") &&
        _tokens.Current!.Value == @operator;

    private ScriptBody Script() =>
        new(StatementList());

    private IEnumerable<StatementListItem> StatementList()
    {
        var statementList = new List<StatementListItem>();
        while (
            CurrentIsKeyword("function") || CurrentIsKeyword("let") || CurrentIsKeyword("const") ||
            CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") ||
            CurrentIsOperator("-") || CurrentIsOperator("!") || CurrentIsOperator("~") ||
            CurrentIs("LeftCurl") || CurrentIsKeyword("return") || CurrentIsKeyword("break") ||
            CurrentIsKeyword("continue") || CurrentIsKeyword("if") || CurrentIsKeyword("while") ||
            CurrentIsKeyword("type")
        )
        {
            statementList.Add(StatementListItem());
        }

        return statementList;
    }

    private StatementListItem StatementListItem()
    {
        if (CurrentIsKeyword("function") || CurrentIsKeyword("let") ||
            CurrentIsKeyword("const") || CurrentIsKeyword("type"))
        {
            return Declaration();
        }

        return Statement();
    }

    private Statement Statement()
    {
        if (CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen") || CurrentIsOperator("-") ||
            CurrentIsOperator("!") || CurrentIsOperator("~"))
        {
            return ExpressionStatement();
        }

        if (CurrentIs("LeftCurl"))
        {
            return BlockStatement();
        }

        if (CurrentIsKeyword("return"))
        {
            return ReturnStatement();
        }

        if (CurrentIsKeyword("break"))
        {
            return new InsideStatementJump(InsideStatementJump.Break)
            {
                Segment = Expect("Keyword", "break").Segment
            };
        }

        if (CurrentIsKeyword("continue"))
        {
            return new InsideStatementJump(InsideStatementJump.Continue)
            {
                Segment = Expect("Keyword", "continue").Segment
            };
        }

        if (CurrentIsKeyword("if"))
        {
            return IfStatement();
        }

        if (CurrentIsKeyword("while"))
        {
            return WhileStatement();
        }

        return null;
    }

    private BlockStatement BlockStatement()
    {
        Expect("LeftCurl");
        var block = new BlockStatement(StatementList());
        Expect("RightCurl");

        return block;
    }

    private ExpressionStatement ExpressionStatement()
    {
        return new(Expression());
    }

    private ReturnStatement ReturnStatement()
    {
        var ret = Expect("Keyword", "return");
        if (CurrentIs("Ident") || CurrentIsLiteral() || CurrentIs("LeftParen")||
            CurrentIsOperator("-") || CurrentIsOperator("!") || CurrentIsOperator("~") ||
            CurrentIs("LeftCurl") || CurrentIs("LeftBracket"))
        {
            return new ReturnStatement(Expression()) { Segment = ret.Segment };
        }

        return new ReturnStatement { Segment = ret.Segment };
    }

    private IfStatement IfStatement()
    {
        var token = Expect("Keyword", "if");
        Expect("LeftParen");
        var expr = Expression();
        Expect("RightParen");
        var then = Statement();
        if (CurrentIsKeyword("else"))
        {
            Expect("Keyword", "else");
            var @else = Statement();
            return new IfStatement(expr, then, @else) {Segment = token.Segment};
        }

        return new IfStatement(expr, then) {Segment = token.Segment};
    }

    private WhileStatement WhileStatement()
    {
        var token = Expect("Keyword", "while");
        Expect("LeftParen");
        var expr = Expression();
        Expect("RightParen");
        var stmt = Statement();
        return new WhileStatement(expr, stmt) {Segment = token.Segment};
    }

    private TypeDeclaration TypeDeclaration()
    {
        var typeWord = Expect("Keyword", "type");
        var ident = Expect("Ident");
        Expect("Assign");
        var type = TypeValue();

        var typeId = new IdentifierReference(name: ident.Value)
            { Segment = ident.Segment };

        return new TypeDeclaration(typeId, type) { Segment = typeWord.Segment + ident.Segment };
    }

    private TypeValue TypeValue()
    {
        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            var identType = new TypeIdentValue(
                TypeId: new IdentifierReference(name: ident.Value)
                    { Segment = ident.Segment });

            return WithSuffix(identType);
        }

        if (CurrentIs("LeftCurl"))
        {
            Expect("LeftCurl");
            var propertyTypes = new List<PropertyTypeValue>();
            while (CurrentIs("Ident"))
            {
                var ident = Expect("Ident");
                Expect("Colon");
                var propType = TypeValue(); 
                propertyTypes.Add(
                    new PropertyTypeValue(
                        ident.Value,
                        propType));
                Expect("SemiColon");
            }

            Expect("RightCurl");
                
            return WithSuffix(new ObjectTypeValue(propertyTypes));
        }

        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var args = new List<TypeValue>();
            while (CurrentIs("Ident") ||
                   CurrentIs("LeftCurl") ||
                   CurrentIs("LeftParen"))
            {
                args.Add(TypeValue());
                if (!CurrentIs("RightParen"))
                {
                    Expect("Comma");
                }
            }
            Expect("RightParen");
            Expect("Arrow");
            var returnType = TypeValue();
            return new FunctionTypeValue(returnType, args);
        }

        return null;
    }

    private TypeValue WithSuffix(TypeValue baseType)
    {
        var type = baseType;
        while (CurrentIs("LeftBracket") || CurrentIs("QuestionMark"))
        {
            if (CurrentIs("LeftBracket"))
            {
                Expect("LeftBracket");
                Expect("RightBracket");
                type = new ArrayTypeValue(type);
            } 
            else if (CurrentIs("QuestionMark"))
            {
                Expect("QuestionMark");
                type = new NullableTypeValue(type);
            }
        }

        return type;
    }

    private Declaration Declaration()
    {
        if (CurrentIsKeyword("function"))
        {
            return FunctionDeclaration();
        }

        if (CurrentIsKeyword("let") || CurrentIsKeyword("const"))
        {
            return LexicalDeclaration();
        }
        
        if (CurrentIsKeyword("type"))
        {
            return TypeDeclaration();
        }

        return null;
    }

    private FunctionDeclaration FunctionDeclaration()
    {
        Expect("Keyword", "function");
        var ident = Expect("Ident");

        Expect("LeftParen");
        var args = new List<PropertyTypeValue>();
        if (CurrentIs("Ident"))
        {
            var arg = Expect("Ident").Value;
            Expect("Colon");
            var type = TypeValue();
            args.Add(new PropertyTypeValue(arg, type));
        }

        while (CurrentIs("Comma"))
        {
            Expect("Comma");
            var arg = Expect("Ident").Value;
            Expect("Colon");
            var type = TypeValue();
            args.Add(new PropertyTypeValue(arg, type));
        }

        var rp = Expect("RightParen");

        TypeValue returnType = new TypeIdentValue(
            TypeId: new IdentifierReference(name: "undefined")
                { Segment = rp.Segment });

        if (CurrentIs("Colon"))
        {
            Expect("Colon");
            returnType = TypeValue();
        }

        var name = new IdentifierReference(ident.Value) { Segment = ident.Segment };
        return new FunctionDeclaration(name, returnType, args, BlockStatement())
            { Segment = ident.Segment };
    }

    private LexicalDeclaration LexicalDeclaration()
    {
        var readOnly = CurrentIsKeyword("const");
        Expect("Keyword", readOnly ? "const" : "let");
        var declaration = new LexicalDeclaration(readOnly);

        AddToDeclaration(declaration);

        while (CurrentIs("Comma"))
        {
            Expect("Comma");
            AddToDeclaration(declaration);
        }

        return declaration;
    }

    private void AddToDeclaration(LexicalDeclaration declaration)
    {
        var ident = Expect("Ident");
        var identRef = new IdentifierReference(ident.Value) { Segment = ident.Segment };
        var assignment = new AssignmentExpression(
                new MemberExpression(identRef),
                new ImplicitLiteral(
                    new TypeIdentValue(
                        new IdentifierReference("undefined"))))
            { Segment = ident.Segment };

        if (CurrentIs("Assign"))
        {
            var assignSegment = Expect("Assign").Segment;
            var expression = Expression();
            assignment = new AssignmentExpression(
                new MemberExpression(identRef), expression
            ) { Segment = assignSegment };
        }
        else if (CurrentIs("Colon"))
        {
            Expect("Colon");
            var type = TypeValue();
            if (CurrentIs("Assign"))
            {
                var assignSegment = Expect("Assign").Segment;
                var expression = Expression();
                assignment = new AssignmentExpression(
                    new MemberExpression(identRef),
                    expression, type
                ) { Segment = assignSegment };
            }
            else
            {
                var expression = new ImplicitLiteral(type);
                assignment = new AssignmentExpression(
                    lhs: new MemberExpression(identRef),
                    expression,
                    type);
            }
        }
        declaration.AddAssignment(assignment);
    }

    private Expression Expression()
    {
        var expr = CastExpression();
        if (expr is LeftHandSideExpression lhs && CurrentIs("Assign"))
        {
            var assign = Expect("Assign");
            return new AssignmentExpression(lhs, Expression())
                {Segment = assign.Segment};
        }
        return expr;
    }

    private Expression CallExpression()
    {
        var member = MemberExpression();
        if (CurrentIs("LeftParen"))
        {
            var lp = Expect("LeftParen");
            var expressions = new List<Expression>();
            if (CurrentIs("Ident") || CurrentIsLiteral() ||
                CurrentIs("LeftParen") || CurrentIsOperator("-") ||
                CurrentIsOperator("!") || CurrentIsOperator("~") ||
                CurrentIs("LeftCurl") || CurrentIs("LeftBracket"))
            {
                expressions.Add(Expression());
            }

            while (CurrentIs("Comma"))
            {
                Expect("Comma");
                expressions.Add(Expression());
            }

            Expect("RightParen");
            return new CallExpression(member as MemberExpression, expressions)
                { Segment = lp.Segment };
        }

        return member;
    }

    private Expression MemberExpression()
    {
        var primary = PrimaryExpression();

        if (!CurrentIs("LeftBracket") && !CurrentIs("Dot") &&
            !CurrentIs("Assign") && !CurrentIs("LeftParen"))
            return primary;

        var identRef = primary as IdentifierReference;
        var accessChain = new List<AccessExpression>();
        while (CurrentIs("LeftBracket") || CurrentIs("Dot"))
        {
            Token access;
            if (CurrentIs("LeftBracket"))
            {
                access = Expect("LeftBracket");
                var lb = access.Segment;
                var expr = Expression();
                var rb = Expect("RightBracket").Segment;
                accessChain.Add(
                    new IndexAccess(expr, accessChain.LastOrDefault()) {Segment = lb + rb}
                );
            }
            else if (CurrentIs("Dot"))
            {
                access = Expect("Dot");
                var identToken = Expect("Ident");
                var idRef = new IdentifierReference(identToken.Value)
                    { Segment = identToken.Segment };
                accessChain.Add(
                    new DotAccess(idRef, accessChain.LastOrDefault()) {Segment = access.Segment}
                );
            }
        }

        return new MemberExpression(identRef, accessChain.FirstOrDefault(), accessChain.LastOrDefault());
    }

    private Expression CastExpression()
    {
        var cond = ConditionalExpression();
        if (CurrentIsKeyword("as"))
        {
            var asKeyword = Expect("Keyword", "as");
            var type = TypeValue();
            return new CastAsExpression(cond, type) {Segment = asKeyword.Segment};
        }

        return cond;
    }

    private Expression ConditionalExpression()
    {
        var test = OrExpression();
        if (CurrentIs("QuestionMark"))
        {
            Expect("QuestionMark");
            var consequent = Expression();
            Expect("Colon");
            var alternate = Expression();
            return new ConditionalExpression(test, consequent, alternate);
        }

        return test;
    }

    private Expression OrExpression()
    {
        var left = AndExpression();
        while (CurrentIsOperator("||"))
        {
            var op = Expect("Operator");
            var right = AndExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression AndExpression()
    {
        var left = EqualityExpression();
        while (CurrentIsOperator("&&"))
        {
            var op = Expect("Operator");
            var right = EqualityExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression EqualityExpression()
    {
        var left = RelationExpression();
        while (CurrentIsOperator("==") || CurrentIsOperator("!="))
        {
            var op = Expect("Operator");
            var right = RelationExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression RelationExpression()
    {
        var left = AdditiveExpression();
        while (CurrentIsOperator(">") || CurrentIsOperator("<") || CurrentIsOperator(">=") ||
               CurrentIsOperator("<="))
        {
            var op = Expect("Operator");
            var right = AdditiveExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression AdditiveExpression()
    {
        var left = MultiplicativeExpression();
        while (CurrentIsOperator("+") || CurrentIsOperator("-"))
        {
            var op = Expect("Operator");
            var right = MultiplicativeExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression MultiplicativeExpression()
    {
        var left = UnaryExpression();
        while (CurrentIsOperator("*") || CurrentIsOperator("/") || CurrentIsOperator("%")
               || CurrentIsOperator("++") || CurrentIsOperator("::"))
        {
            var op = Expect("Operator");
            var right = UnaryExpression();
            left = new BinaryExpression(left, op.Value, right)
            {
                Segment = op.Segment
            };
        }

        return left;
    }

    private Expression UnaryExpression()
    {
        if (CurrentIsOperator("-") || CurrentIsOperator("!") || CurrentIsOperator("~"))
        {
            var op = Expect("Operator");
            return new UnaryExpression(op.Value, UnaryExpression())
            {
                Segment = op.Segment
            };
        }

        return LeftHandSideExpression();
    }
    
    private Expression LeftHandSideExpression()
    {
        return CallExpression();
    }

    private Expression PrimaryExpression()
    {
        if (CurrentIs("LeftParen"))
        {
            Expect("LeftParen");
            var expr = Expression();
            Expect("RightParen");
            return expr;
        }

        if (CurrentIs("Ident"))
        {
            var ident = Expect("Ident");
            var id = new IdentifierReference(ident.Value)
            {
                Segment = ident.Segment
            };

            return id;
        }

        if (CurrentIsLiteral())
        {
            return Literal();
        }

        if (CurrentIs("LeftCurl"))
        {
            return ObjectLiteral();
        }
            
        if (CurrentIs("LeftBracket"))
        {
            return ArrayLiteral();
        }

        return null;
    }

    private Literal Literal()
    {
        var segment = _tokens.Current!.Segment;
        if (CurrentIs("StringLiteral"))
        {
            var str = Expect("StringLiteral");
            return new Literal(
                new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "string")
                        {Segment = str.Segment}),
                value: Regex.Unescape(str.Value.Trim('"')),
                segment,
                label: str.Value
                    .Replace(@"\", @"\\")
                    .Replace(@"""", @"\"""));
        }

        return _tokens.Current.Type.Tag switch
        {
            "NullLiteral" => new Literal(
                new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "null")
                        { Segment = _tokens.Current.Segment }),
                Expect("NullLiteral").Value == "null" ? null : string.Empty,
                segment,
                label: "null"),
            "IntegerLiteral" => new Literal(
                new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "number")
                        { Segment = _tokens.Current.Segment }),
                value: double.Parse(Expect("IntegerLiteral").Value),
                segment),
            "FloatLiteral" => new Literal(
                new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "number")
                        { Segment = _tokens.Current.Segment }),
                value: double.Parse(
                    Expect("FloatLiteral").Value,
                    CultureInfo.InvariantCulture),
                segment),
            "BooleanLiteral" => new Literal(
                new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "boolean")
                        { Segment = _tokens.Current.Segment }),
                value: bool.Parse(Expect("BooleanLiteral").Value),
                segment),
            _ => throw new ParserException("There are no more supported literals")
        };
    }

    private ObjectLiteral ObjectLiteral()
    {
        Expect("LeftCurl");
        var properties = new List<Property>();
        var methods = new List<FunctionDeclaration>();
        while (CurrentIs("Ident"))
        {
            var idToken = Expect("Ident");
            var id = new IdentifierReference(idToken.Value)
                { Segment = idToken.Segment };
            if (CurrentIs("Colon"))
            {
                Expect("Colon");
                var expr = Expression();
                properties.Add(new Property(id, expr));
            }
            else if (CurrentIs("Arrow"))
            {
                Expect("Arrow");
                Expect("LeftParen");
                var args = new List<PropertyTypeValue>();
                while (CurrentIs("Ident"))
                {
                    var paramName = Expect("Ident").Value;
                    Expect("Colon");
                    var type = TypeValue();
                    args.Add(new PropertyTypeValue(paramName, type));
                    if (!CurrentIs("RightParen"))
                    {
                        Expect("Comma");
                    }
                }
                var rp = Expect("RightParen");
                TypeValue returnType = new TypeIdentValue(
                    TypeId: new IdentifierReference(name: "undefined")
                        { Segment = rp.Segment });
                if (CurrentIs("Colon"))
                {
                    Expect("Colon");
                    returnType = TypeValue();
                }

                var name = new IdentifierReference(idToken.Value) { Segment = idToken.Segment };
                methods.Add(new FunctionDeclaration(name, returnType, args, BlockStatement())
                    { Segment = idToken.Segment }
                );
            }

            Expect("SemiColon");
        }
        Expect("RightCurl");
        return new ObjectLiteral(properties, methods);
    }

    private ArrayLiteral ArrayLiteral()
    {
        var lb = Expect("LeftBracket").Segment;
        var expressions = new List<Expression>();
        while (CurrentIs("Ident") || CurrentIsLiteral() ||
               CurrentIs("LeftParen") || CurrentIsOperator("-") ||
               CurrentIsOperator("!") || CurrentIsOperator("~") ||
               CurrentIs("LeftCurl") || CurrentIs("LeftBracket"))
        {
            expressions.Add(Expression());
            if (!CurrentIs("RightBracket"))
            {
                Expect("Comma");
            }
        }
        var rb = Expect("RightBracket").Segment;
        return new ArrayLiteral(expressions) {Segment = lb + rb};
    }
}